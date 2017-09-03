using System;
using System.Linq;
using System.IO;
using FileUtil;
using System.Windows.Forms;
namespace FileAtOnceParser
{ //"C:\Users\fujit\Documents\LicoriceAudio_Repo\Deep Dungeon2017\Assets\Data\SkillData\CampData.cs"
	class FileAtOnceParser
	{
		[STAThread]
		static void Main()
		{
			//---クリップボードからファイルパスを取得
			Console.Write("クリップボードにファイルパスを入れてください　\n");
			string path = Clipboard.GetText();
			string[] separator = {"\r\n" };
			var pathList = Clipboard.GetText().Split(separator,StringSplitOptions.RemoveEmptyEntries).ToList();
			pathList.WriteConsole();

			Console.Write("置換前文字列を入力してください　\n");
			string searchKey = Console.ReadLine();
			Console.Write("置換後文字列を入力してください　[FileName] = ファイルの名前を置換に使用\n");
			string replaceKey = Console.ReadLine();

			for(int i = 0; i < pathList.Count; i++)
			{
				string _path = pathList[i].Trim('\"');
				string tempPath = _path+"temp";
				StreamReader streamReader = new StreamReader(File.OpenRead(_path));
				StreamWriter streamWriter = new StreamWriter(File.OpenWrite(tempPath));

				string nowLine ="";
				do
				{
					nowLine = streamReader.ReadLine();
					if(nowLine!=null)
					{
						if(nowLine.Contains(searchKey))
						{
							string _replaceKey = null;
							if(replaceKey.Contains("[FileName]"))
							{
								_replaceKey = replaceKey.Replace("[FileName]",pathList[i].GetFileName());
							}
							string _searchKey = searchKey;
							string replacedData = nowLine.Replace(_searchKey,_replaceKey);
							streamWriter.WriteLine(replacedData);
						}
						else
						{
							streamWriter.WriteLine(nowLine);
						}
					}
					
				} while(nowLine!=null);
				streamWriter.Close();
				streamReader.Close();

				File.Replace(tempPath,_path,tempPath+"temp");
				Console.WriteLine($"{_path} was Replaced");
			}

			Console.Read();


		}
	}
}
