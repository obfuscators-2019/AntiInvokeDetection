using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet.Emit;
using dnlib.DotNet;
using System.Reflection;
using dnlib.DotNet.Writer;

namespace Anti_Invoke_Detection
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleShort Console = new ConsoleShort();
            if (asArgs(args))
            {
                try
                {
                    int count = 0;
                    ModuleDefMD md = ModuleDefMD.Load(args[0]);
                    foreach (TypeDef type in md.GetTypes())
                    {
                        if (!type.IsGlobalModuleType) continue;
                        foreach (MethodDef method in type.Methods)
                        {
                            try
                            {
                                if (!method.HasBody && !method.Body.HasInstructions) continue;
                                for (int i = 0; i < method.Body.Instructions.Count; i++)
                                {
                                    if (method.Body.Instructions[i].OpCode == OpCodes.Call && method.Body.Instructions[i].Operand.ToString().Contains("CallingAssembly"))
                                    {
                                        method.Body.Instructions[i].Operand = (method.Body.Instructions[i].Operand = md.Import(typeof(Assembly).GetMethod("GetExecutingAssembly")));
                                        count++;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    Console.WriteLine("Invoke Detection replaced: " + count, ConsoleColor.Red);
                    ModuleWriterOptions writerOptions = new ModuleWriterOptions(md);
                    writerOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
                    writerOptions.Logger = DummyLogger.NoThrowInstance;

                    NativeModuleWriterOptions NativewriterOptions = new NativeModuleWriterOptions(md);
                    NativewriterOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
                    NativewriterOptions.Logger = DummyLogger.NoThrowInstance;
                    if (md.IsILOnly)
                        md.Write(args[0] + "_unpacked.exe", writerOptions);
                    
                    md.NativeWrite(args[0] + "_unpacked.exe", NativewriterOptions);
                }
                catch { Console.WriteLine("File isn't .net assembly valid! ", ConsoleColor.Red); }
            }
            else
            {
                Console.WriteLine("No Args Detected!", ConsoleColor.Red);
            }
            Console.Pause();
        }
        static bool asArgs(string[] args)
        {
            try
            {
                if (args[0] != "2xx1452851xx821x") { return true; } else { return false; }
            }
            catch { return false; }
        }
    }
}
