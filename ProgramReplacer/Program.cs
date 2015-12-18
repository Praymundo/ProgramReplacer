using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgramReplacer
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int MessageBoxW(int hWnd, String text, String caption, uint type);

        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    String fileName = System.IO.Path.GetFileName(args[0]).ToLower();
                    String toCall = (string)System.Configuration.ConfigurationSettings.AppSettings[fileName];

                    if (toCall == "")
                    {
                        throw new Exception("Nenhum programa configurado para ser chamado: " + fileName);
                    }

                    if (!System.IO.File.Exists(toCall))
                    {
                        throw new Exception("O programa a ser chamado não foi encontrado: " + toCall);
                    }

                    if (fileName == System.IO.Path.GetFileName(toCall).ToLower())
                    {
                        throw new Exception("O programa a ser chamado não pode ser o mesmo que foi chamado.");
                    }

                    StringBuilder parameters = new StringBuilder();
                    if (args.Length > 1)
                    {
                        for (int i = 1; i < args.Length; i++)
                        {
                            if (parameters.Length > 0) { parameters.Append(" "); }
                            parameters.Append(@"""" + args[i].Replace(@"""", @"\""") + @"""");
                        }
                    }

                    System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo();
                    procStartInfo.FileName = toCall;
                    procStartInfo.Arguments = parameters.ToString();
                    System.Diagnostics.Process.Start(procStartInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBoxW(0, ex.Message, "Program Replacer", 0);
            }
        }
    }
}
