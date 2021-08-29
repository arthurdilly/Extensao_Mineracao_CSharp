using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

/*-------------CLASSES----------------*/
public class Arquivos
{
    public static void Ex1(string PathString, string[] lines)
    {
        using StreamWriter file = new (PathString);    //Abre o arquivo de PathString em file para a escrita
        foreach(string line in lines)
        {
            file.WriteLine(line);
        }
        
    }

    public static void Ex2(string PathString, string[] lines)
    {
        using StreamWriter file = new (PathString);    //Abre o arquivo de PathString em file para a escrita
        foreach(string line in lines)                  //Cada linha é uma string
        {
            if(!line.Contains("2"))                    //Se a linha não contem o caractere "2", escreve a linha
            {
                file.WriteLine(line);
            }
            
        }
    }

    public static void Ex3(string PathString, string[] lines)
    {
        using StreamWriter file = new StreamWriter(PathString, append : true);//Abre o arquivo de PathString em file para a escrita
                                                                   //Append:true -> não recria o arquivo. Escreve no final
        foreach(string line in lines)                              //Cada linha é uma string
        {
            if(line.Contains("est"))                               //Se a linha não contem a sequencia "Test", escreve a linha
            {
                file.WriteLine(line);
            }
            
        }
    }

    //Exemplo com leitura de arquivos PDF
    public static void Ex4()
    {
        using(PdfReader reader = new PdfReader(@"C:\Users\Public\Documents\Mineracao_C#\Enunciado - Projeto 1.pdf"))
        {
                var texto = new System.Text.StringBuilder();
                using System.IO.StreamWriter file = new StreamWriter(@"C:\Users\Public\Documents\Mineracao_C#\TestePDF.txt", append : true);

                for(int i = 1; i <= reader.NumberOfPages; i++)          //NumberOfPages = atributo do objeto "reader"
                {
                    string aux = PdfTextExtractor.GetTextFromPage(reader, i);   //Extrai o texto de "reader" da pagina "i". Texto da pagina inteira
                    string[] linhas = aux.Split('\n');                          //Faz a quebra de linhas. Uma linha por posição do vetor.
                                                                                //Da primeira posicao da linha ate encontrar "\n"

                    //Procurar informações no PDF
                    foreach(string linha in linhas)
                    {
                        if(linha.Contains(@"projeto não") || linha.Contains(@"Projeto não")) //Se a linha conter XXXXX ou XXXXX
                        {
                            texto.Append($"{linha}{'\n'}");                       //Adicina à variavel texto o conteudo da linha e uma quebra de linha
                            file.WriteLine(linha);                              //Escreve a linha no arquivo txt
                        }
                    }
                }

                Console.Write(texto);                                           //Imprime no console o texto completo
        }

    }




   
        static void Main(string[] args)
        {
            string NomePasta = @"C:\Users\Public\Documents\Mineracao_C#"; //Caminho para a pasta

            //Combina para criar nova pasta
            string PathString = System.IO.Path.Combine(NomePasta, "Projeto_1");

            System.IO.Directory.CreateDirectory(PathString);

            string NomeArquivo = "ArquivoTeste.txt";

            PathString = System.IO.Path.Combine(PathString, NomeArquivo);

            Console.WriteLine("O caminho do arquivo é {0} \n", PathString);
            
            //Testa se o arquivo existe
            if(!System.IO.File.Exists(PathString))
            {
                //Console.WriteLine("O arquivo não existe.");
                //Caso o arquivo não exista
                //Informa que file será o arquivo no caminho de pathstring
                //Cria o arquivo texto e escreve nele
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(PathString))//Vincula o arquivo ao stream
                {
                    for(byte i = 0; i < 100; i++)                                           //Escreve o byte criado
                    {
                        file.WriteLine($"{i}");                                             //Escreve i (local da variavel deve estar entre {})
                    }

                    Console.WriteLine("Finalizado");
                }
            }
            else{

                    Console.WriteLine("O arquivo {0} já existe.", NomeArquivo);
            }

            try
            {
                foreach(string line in File.ReadLines(PathString))  //Pega as linhas do arquivo em patchstring e escreve
                {
                    Console.WriteLine(line);
                }
            }
            catch(System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }  

            //Matriz com tres string
            string[] lines = {"Testando exemplo de string maior", "Linha 2", "Linha 3"};

            NomeArquivo = "TesteEx1.txt";                          //Altera o nome do arquivo
            PathString = System.IO.Path.Combine(NomePasta, NomeArquivo);

            Ex1(PathString, lines);

            NomeArquivo = "TesteEx2.txt";
            PathString = System.IO.Path.Combine(NomePasta, NomeArquivo);
            Ex2(PathString, lines);

            NomeArquivo = "TesteEx3.txt";
            PathString = System.IO.Path.Combine(NomePasta, NomeArquivo);
            Ex3(PathString, lines);

            Ex4();
        }//end main

}  

