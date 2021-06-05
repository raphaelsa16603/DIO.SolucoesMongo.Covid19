using System;
using System.Text;

namespace LibConsoleProgressBar
{
    public class ToolsProgressBar
    {
        private string currentText = string.Empty;
        
        public void UpdateText(string text) 
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength]) {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0) {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
	    }

        public string BarraProgressoTexto(char simbolo, int tamanho, double percentagem) 
        {
            int progress = 0;

            if(percentagem > 0)
            {
                progress = (int) Math.Round(
                        (tamanho * (percentagem/100)),0);
            }
            else if (percentagem == 0 )
                progress = 0;
            else if (percentagem < 0 )
                progress = 0;
            else if (percentagem > 100 )
                progress = 0;

			int percent = (int) (percentagem * 100);
            string text = string.Format("[{0}{1}]",
				new string('#', progress), 
                new string('-', Math.Abs(tamanho - progress)));

            return text;
        }
    }
}