using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Linq;

namespace FineCover
{
	public static class Instrumentation
	{
		private static string INPUT_DLL_FILE;
		private static string OUTPUT_DLL_FILE;
		private static string OUTPUT_COVER_FILE;
		private static readonly Type THIS_TYPE = typeof(Instrumentation);

		public static void Hit(string hit)
		{
			File.AppendAllText(OUTPUT_COVER_FILE, $"{hit}{Environment.NewLine}");
		}

		public static void Instrument(string inputDllFile, string outputDllFile = null, string outputCoverFile = null)
		{
			if (string.IsNullOrWhiteSpace(inputDllFile))
			{
				throw new ArgumentNullException(nameof(inputDllFile));
			}

			if (!File.Exists(inputDllFile))
			{
				throw new FileNotFoundException(inputDllFile);
			}

			INPUT_DLL_FILE = inputDllFile;
			OUTPUT_DLL_FILE = outputDllFile ?? $"{inputDllFile}.FineCover.dll";
			OUTPUT_COVER_FILE = outputCoverFile ?? $"{inputDllFile}.FineCover.txt";

			if (File.Exists(OUTPUT_DLL_FILE))
			{
				File.Delete(OUTPUT_DLL_FILE);
			}

			if (File.Exists(OUTPUT_COVER_FILE))
			{
				File.Delete(OUTPUT_COVER_FILE);
			}

			var module = ModuleDefinition.ReadModule(INPUT_DLL_FILE, new ReaderParameters
			{
				ReadWrite = true,
				ReadSymbols = true,
			});

			module.Assembly.Name.Name = Path.GetFileNameWithoutExtension(OUTPUT_DLL_FILE);

			var hitMethod = module.ImportReference(typeof(Instrumentation).GetMethod(nameof(Hit), new[]
			{
				typeof(string)
			}));
			
			foreach (var type in module.Types)
			{
				foreach (var method in type.Methods)
				{
					if (!method.HasBody || !method.DebugInformation.HasSequencePoints)
					{
						continue;
					}

					var ilProcessor = method.Body.GetILProcessor();

					var codeSegments = method
						.DebugInformation
						.SequencePoints
						.Select(sp => new CodeSegment { FilePath = sp.Document.Url, EndLine = sp.EndLine })
						.Where(cs => cs.EndLine != 16707566) // https://github.com/OpenCppCoverage/OpenCppCoverage/issues/97
						.Distinct(new CodeSegmentComparer())
						.ToArray();

					foreach (var codeSegment in codeSegments)
					{
						var sequencePointsWithSameEndLine = method.DebugInformation.SequencePoints.Where(x => x.EndLine == codeSegment.EndLine);

						codeSegment.StartLine = sequencePointsWithSameEndLine.Min(x => x.StartLine);
						codeSegment.Instructions = sequencePointsWithSameEndLine.Select(x => (Instruction)x.GET("offset").GET("instruction")).ToArray();

						var last = codeSegment.Instructions.Last();

						ilProcessor.InsertAfter(last, last = Instruction.Create(OpCodes.Ldstr, $"{codeSegment.FilePath}|{method.FullName}|{codeSegment.StartLine}|{codeSegment.EndLine}"));
						ilProcessor.InsertAfter(last, last = Instruction.Create(OpCodes.Call, hitMethod));
					}
				}
			}

			module.Write(OUTPUT_DLL_FILE, new WriterParameters
			{
				WriteSymbols = false,
			});
		}
	}
}
