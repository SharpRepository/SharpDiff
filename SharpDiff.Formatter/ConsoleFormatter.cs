using System;

namespace SharpDiff.Formatter
{
    public class ConsoleFormatter : BaseFormatter
    {
        public override void Execute(Diff diff)
        {
            foreach (var file in diff.Files)
            {
                Console.WriteLine("---------------------");
                Console.WriteLine(file.FileName);
                Console.WriteLine("---------------------");

                foreach (var section in file.FileSections)
                {
                    Console.WriteLine(section.Description);

                    if (file.FileChangeType == FileChangeType.Deleted)
                    {
                        if (!ShowDeletedFileContents)
                        {
                            Console.WriteLine("[File Deleted]");

                            continue;
                        }
                    }

                    if (file.FileChangeType == FileChangeType.Created)
                    {
                        if (!ShowCreatedFileContents)
                        {
                            Console.WriteLine("[New File]");

                            continue;
                        }
                    }

                    foreach (var line in section.Lines)
                    {
                        switch (line.LineChangeType)
                        {
                            case LineChangeType.Add:
                                Console.Write("++++ ");
                                break;
                            case LineChangeType.Delete:
                                Console.Write("---- ");
                                break;
                            default:
                                Console.Write("     ");
                                break;
                        }

                        Console.WriteLine(line.Text);
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
