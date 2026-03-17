namespace Vision_Master.Core
{
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

public enum IniTextEncoding
{
    Auto = 0,
    Ansi = 1,      // System default ANSI code page
    Unicode = 2,   // UTF-16 LE with BOM (supported by Win32 profile APIs)
    Utf8 = 3       // UTF-8 with BOM (handled by managed parser)
}

public class IniFile
{
    private readonly string _path;
    private readonly IniTextEncoding _encodingMode;

    public IniFile(string path)
        : this(path, IniTextEncoding.Auto)
    {
    }

    public IniFile(string path, IniTextEncoding encodingMode)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path is null or empty.", nameof(path));
        _path = System.IO.Path.GetFullPath(path);
        _encodingMode = encodingMode;
    }

    public string FilePath => _path;

    /// <summary>
    /// Desired file encoding mode for this INI file.
    /// Auto: detect existing; Ansi: system code page; Unicode: UTF-16 LE with BOM; Utf8: UTF-8 with BOM.
    /// </summary>
    public IniTextEncoding EncodingMode => _encodingMode;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedString,
        int nSize,
        string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WritePrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        string lpFileName);

    /// <summary>
    /// Read a string value. Returns defaultValue when section/key not found.
    /// Applies configured encoding: Utf8 uses managed parser; others use Win32 profile API.
    /// Auto will switch to managed parser when the file is detected as UTF-8.
    /// </summary>
    public string ReadString(string section, string key, string defaultValue = "")
    {
        if (section == null) throw new ArgumentNullException(nameof(section));
        if (key == null) throw new ArgumentNullException(nameof(key));

        bool useUtf8 = _encodingMode == IniTextEncoding.Utf8 ||
                        (_encodingMode == IniTextEncoding.Auto && IsUtf8File(_path));

        if (useUtf8)
        {
            return ManagedReadString(section, key, defaultValue, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        // Start with a reasonable buffer and grow if needed for Win32 API path
        int size = 256;
        var sb = new StringBuilder(size);
        while (true)
        {
            sb.Clear();
            int chars = GetPrivateProfileString(section, key, defaultValue ?? string.Empty, sb, sb.Capacity, _path);
            if (chars < sb.Capacity - 1)
            {
                return sb.ToString();
            }
            // Buffer was likely too small; grow and retry.
            size = checked(sb.Capacity * 2);
            sb = new StringBuilder(size);
        }
    }

    /// <summary>
    /// Read an int value. Returns defaultValue when missing or parse fails.
    /// </summary>
    public int ReadInt(string section, string key, int defaultValue = 0)
    {
        var s = ReadString(section, key, defaultValue.ToString(CultureInfo.InvariantCulture));
        int result;
        return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? result : defaultValue;
    }

    /// <summary>
    /// Read a double value. Returns defaultValue when missing or parse fails.
    /// </summary>
    public double ReadDouble(string section, string key, double defaultValue = 0.0)
    {
        var s = ReadString(section, key, defaultValue.ToString("R", CultureInfo.InvariantCulture));
        double result;
        return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result) ? result : defaultValue;
    }

    /// <summary>
    /// Read a boolean value. Accepts true/false, 1/0, yes/no, on/off (case-insensitive).
    /// Returns defaultValue when missing or parse fails.
    /// </summary>
    public bool ReadBool(string section, string key, bool defaultValue = false)
    {
        var s = ReadString(section, key, defaultValue ? "1" : "0");
        if (string.IsNullOrWhiteSpace(s)) return defaultValue;

        switch (s.Trim().ToLowerInvariant())
        {
            case "1":
            case "true":
            case "yes":
            case "on":
                return true;
            case "0":
            case "false":
            case "no":
            case "off":
                return false;
            default:
                bool parsed;
                return bool.TryParse(s, out parsed) ? parsed : defaultValue;
        }
    }

    /// <summary>
    /// Write a string value. Returns true when successful.
    /// Applies configured encoding: Utf8 uses managed writer; others use Win32 profile API.
    /// Auto will use managed writer when the file is detected as UTF-8.
    /// </summary>
    public bool WriteString(string section, string key, string value)
    {
        if (section == null) throw new ArgumentNullException(nameof(section));
        if (key == null) throw new ArgumentNullException(nameof(key));

        bool useUtf8 = _encodingMode == IniTextEncoding.Utf8 ||
                        (_encodingMode == IniTextEncoding.Auto && IsUtf8File(_path));

        EnsureDirectoryExists();

        if (useUtf8)
        {
            EnsureFileEncodingForWrite(utf8Preferred: true);
            return ManagedWriteString(section, key, value ?? string.Empty, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        EnsureFileEncodingForWrite(utf8Preferred: false);
        var ok = WritePrivateProfileString(section, key, value ?? string.Empty, _path);
        // Flush the cached profile string data to disk.
        WritePrivateProfileString(null, null, null, _path);
        return ok;
    }

    /// <summary>
    /// Write an int value. Returns true when successful.
    /// </summary>
    public bool WriteInt(string section, string key, int value)
    {
        return WriteString(section, key, value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Write a double value (round-trip format). Returns true when successful.
    /// </summary>
    public bool WriteDouble(string section, string key, double value)
    {
        // Use round-trip format to preserve precision.
        return WriteString(section, key, value.ToString("R", CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Write a boolean value as 1/0. Returns true when successful.
    /// </summary>
    public bool WriteBool(string section, string key, bool value)
    {
        return WriteString(section, key, value ? "1" : "0");
    }

    /// <summary>
    /// Check if the INI file exists on disk.
    /// </summary>
    public bool Exists() => File.Exists(_path);

    private void EnsureDirectoryExists()
    {
        var dir = System.IO.Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    private void EnsureFileEncodingForWrite(bool utf8Preferred)
    {
        try
        {
            if (!File.Exists(_path))
            {
                // Initialize new file with desired encoding
                if (_encodingMode == IniTextEncoding.Utf8 || utf8Preferred)
                {
                    using (var fs = new FileStream(_path, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                    using (var writer = new StreamWriter(fs, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
                    {
                        writer.Write(string.Empty); // emit BOM
                    }
                }
                else
                {
                    switch (_encodingMode)
                    {
                        case IniTextEncoding.Unicode:
                            using (var fs = new FileStream(_path, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                            using (var writer = new StreamWriter(fs, Encoding.Unicode))
                            {
                                writer.Write(string.Empty); // emit BOM
                            }
                            break;
                        case IniTextEncoding.Ansi:
                            using (var fs = new FileStream(_path, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                            using (var writer = new StreamWriter(fs, Encoding.Default))
                            {
                                writer.Write(string.Empty);
                            }
                            break;
                        case IniTextEncoding.Auto:
                        default:
                            // Let OS create it on first write
                            break;
                    }
                }
                return;
            }

            // If file exists and mode explicitly requested, adjust if needed (best-effort)
            if (_encodingMode == IniTextEncoding.Unicode && !IsUnicodeFile(_path))
            {
                ConvertFileEncoding(_path, GuessEncoding(_path), Encoding.Unicode);
            }
            else if (_encodingMode == IniTextEncoding.Ansi && (IsUnicodeFile(_path) || IsUtf8File(_path)))
            {
                ConvertFileEncoding(_path, GuessEncoding(_path), Encoding.Default);
            }
            else if (_encodingMode == IniTextEncoding.Utf8 && !IsUtf8File(_path))
            {
                ConvertFileEncoding(_path, GuessEncoding(_path), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            }
        }
        catch
        {
            // Best-effort only; ignore encoding adjustments errors.
        }
    }

    private static bool IsUnicodeFile(string path)
    {
        if (!File.Exists(path)) return false;
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            if (fs.Length < 2) return false;
            int b1 = fs.ReadByte();
            int b2 = fs.ReadByte();
            return b1 == 0xFF && b2 == 0xFE; // UTF-16 LE BOM
        }
    }

    private static bool IsUtf8File(string path)
    {
        if (!File.Exists(path)) return false;
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            if (fs.Length < 3) return false;
            int b1 = fs.ReadByte();
            int b2 = fs.ReadByte();
            int b3 = fs.ReadByte();
            return b1 == 0xEF && b2 == 0xBB && b3 == 0xBF; // UTF-8 BOM
        }
    }

    private static Encoding GuessEncoding(string path)
    {
        if (IsUnicodeFile(path)) return Encoding.Unicode;
        if (IsUtf8File(path)) return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
        return Encoding.Default; // assume ANSI
    }

    private static void ConvertFileEncoding(string path, Encoding srcEncoding, Encoding dstEncoding)
    {
        string content;
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(fs, srcEncoding, detectEncodingFromByteOrderMarks: true))
        {
            content = reader.ReadToEnd();
        }
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
        using (var writer = new StreamWriter(fs, dstEncoding))
        {
            writer.Write(content);
        }
    }

    // Managed UTF-8 reader/writer for INI
    private string ManagedReadString(string section, string key, string defaultValue, Encoding encoding)
    {
        try
        {
            if (!File.Exists(_path)) return defaultValue ?? string.Empty;
            var lines = File.ReadAllLines(_path, encoding);
            string currentSection = null;
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.Length == 0) continue;
                if (line.StartsWith(";") || line.StartsWith("#")) continue;
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2).Trim();
                    continue;
                }
                if (!string.Equals(currentSection, section, StringComparison.OrdinalIgnoreCase))
                    continue;

                int eqIndex = line.IndexOf('=');
                if (eqIndex <= 0) continue;
                string k = line.Substring(0, eqIndex).Trim();
                if (!string.Equals(k, key, StringComparison.OrdinalIgnoreCase)) continue;
                string v = line.Substring(eqIndex + 1);
                return v; // keep original spacing/value text after '='
            }
        }
        catch
        {
            // ignore and return default
        }
        return defaultValue ?? string.Empty;
    }

    private bool ManagedWriteString(string section, string key, string value, Encoding encoding)
    {
        try
        {
            var utf8Bom = encoding as UTF8Encoding;
            if (utf8Bom == null)
            {
                encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
            }

            List<string> lines = new List<string>();
            if (File.Exists(_path))
            {
                lines.AddRange(File.ReadAllLines(_path, encoding));
            }

            if (lines.Count == 0)
            {
                // Create new file with section and key
                lines.Add("[" + section + "]");
                lines.Add(key + "=" + (value ?? string.Empty));
                File.WriteAllLines(_path, lines.ToArray(), encoding);
                return true;
            }

            // Locate section
            int secStart = -1;
            int secEnd = lines.Count; // exclusive
            for (int i = 0; i < lines.Count; i++)
            {
                var raw = lines[i];
                var trimmed = raw.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    string name = trimmed.Substring(1, trimmed.Length - 2).Trim();
                    if (secStart >= 0)
                    {
                        // we found next section boundary
                        secEnd = i;
                        if (string.Equals(name, section, StringComparison.OrdinalIgnoreCase))
                        {
                            // new section matches too; but first boundary set, continue
                        }
                    }
                    if (string.Equals(name, section, StringComparison.OrdinalIgnoreCase))
                    {
                        secStart = i;
                        // find end on next iterations
                    }
                    else if (secStart >= 0 && secEnd == lines.Count)
                    {
                        secEnd = i; // end of our section
                    }
                }
            }
            if (secStart >= 0 && secEnd == lines.Count)
            {
                // if section was the last, secEnd remains lines.Count
                // already fine
            }

            if (secStart < 0)
            {
                // Append new section at end
                if (lines.Count > 0 && lines[lines.Count - 1].Trim().Length != 0)
                    lines.Add(string.Empty);
                lines.Add("[" + section + "]");
                lines.Add(key + "=" + (value ?? string.Empty));
                File.WriteAllLines(_path, lines.ToArray(), encoding);
                return true;
            }

            // Search key inside section range
            for (int i = secStart + 1; i < secEnd; i++)
            {
                var raw = lines[i];
                var trimmed = raw.Trim();
                if (trimmed.Length == 0) continue;
                if (trimmed.StartsWith(";") || trimmed.StartsWith("#")) continue;
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]")) break; // safety
                int eqIndex = raw.IndexOf('='); // use raw to preserve left spacing
                if (eqIndex <= 0) continue;
                string k = raw.Substring(0, eqIndex).Trim();
                if (!string.Equals(k, key, StringComparison.OrdinalIgnoreCase)) continue;

                // Replace line preserving key's original left spacing
                string left = raw.Substring(0, eqIndex);
                lines[i] = left + "=" + (value ?? string.Empty);
                File.WriteAllLines(_path, lines.ToArray(), encoding);
                return true;
            }

            // Key not found in section; insert before secEnd
            int insertIndex = secEnd;
            // Skip backward blank lines to keep things tight
            while (insertIndex > secStart + 1 && lines[insertIndex - 1].Trim().Length == 0)
            {
                insertIndex--;
            }
            lines.Insert(insertIndex, key + "=" + (value ?? string.Empty));
            File.WriteAllLines(_path, lines.ToArray(), encoding);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
}
