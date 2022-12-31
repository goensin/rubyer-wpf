using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RemixIconCodeGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var typeStringBuilder = new StringBuilder();
            var dataStringBuilder = new StringBuilder();
            var directories = Directory.GetDirectories("icons");
            foreach (var directory in directories)
            {
                var files = Directory.GetFiles(directory);
                foreach (var file in files)
                {
                    // <path d="M18.901 10a2.999 2.999 0 0 0 4.075 1.113 3.5 3.5 0 0 1-1.975 3.55L21 21h-6v-2a3 3 0 0 0-5.995-.176L9 19v2H3v-6.336a3.5 3.5 0 0 1-1.979-3.553A2.999 2.999 0 0 0 5.098 10h13.803zm-1.865-7a3.5 3.5 0 0 0 4.446 2.86 3.5 3.5 0 0 1-3.29 3.135L18 9H6a3.5 3.5 0 0 1-3.482-3.14A3.5 3.5 0 0 0 6.964 3h10.072z"/>
                    var lines = File.ReadAllLines(file);

                    foreach (var line in lines)
                    {
                        string pattern = "<path d=(.+)/>";
                        Regex regex = new Regex(pattern);
                        var match = regex.Match(line.Replace("fill-rule=\"nonzero\" ", ""));
                        if (match.Success)
                        {
                            var names = Path.GetFileNameWithoutExtension(file).Trim().Split(' ', '-');
                            var firstChar = names[0].First();
                            names[0] = ((firstChar >= 48 && firstChar <= 57) ? "_" : "") + names[0];
                            var name = string.Empty;
                            for (int i = 0; i < names.Length; i++)
                            {
                                name += names[i].Substring(0, 1).ToUpper() + names[i].Substring(1);
                            }

                            var group = new DirectoryInfo(directory).Name;
                            dataStringBuilder.AppendLine($"{{ IconType.{name}, new IconInfo(\"{group}\", {match.Groups[1]})}},");
                            typeStringBuilder.AppendLine($"{name},");
                        }
                    }
                }
            }

            var types = typeStringBuilder.ToString();
            var datas = dataStringBuilder.ToString();
        }
    }
}