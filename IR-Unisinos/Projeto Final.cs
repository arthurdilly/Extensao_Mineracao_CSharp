using System;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Data;
using System.Data.SQLite;

/*
    Nome: Arthur Dilly Escher
    Data 10/09/2021

    Curso de Extensão Mineração de Texto em C# - Projeto Final

    O programa abre todos os documentos, separa as palavras, remove stop words, obtem o vocabulario e quantidade de termos para cada arquivo.
    A matrix "Termos" armazena as palavras encontradas em todos os documentos.
    Cada LINHA da matriz é um documento e os termos estão armazenadas nas COLUNAS.
    Na matriz "Quantidade_Cada_Termos" estão armazenadas as quantidades encontradas para cada termo da matriz "Termos".
    A similaridade é calculada somando a quantidade de cada termo pesquisado e que foi achado no arquivo.

    Não há implementação de stemming e BD.
    
*/
namespace Projeto_Final
{
    class Program
    {
        static void Main()
        {
            
            string Caminho_Documentos;
            
            Console.WriteLine("Digite o caminho dos documentos a serem lidos:");
            Caminho_Documentos = Console.ReadLine();
            string Caminho_Noticias;
            string Caminho_Noticias_Temp;

            int Numero_Documentos = 20;

            Console.WriteLine("Digite as palavras a serem buscadas:");
            //Armazenamento de palavras
            string Words = Console.ReadLine();                                    //Armazena a string de palavras digitada
            string[] Palavras = new string[50];                                   //Armazena as palavras a serem buscadas
            int Quantidade_Palavras = 0;
            string[] Palavras_Separadas = Words.Split(' ');

            for(int i = 0; i < Palavras_Separadas.Length; i++)
            {
                if(!Palavras_Separadas[i].Equals("AND"))                          //Remove operadores AND, caso sejam usados
                {
                    Palavras[Quantidade_Palavras] = Palavras_Separadas[i];
                    Quantidade_Palavras ++;
                }
            }

            for(int i = 0; i < Quantidade_Palavras; i++)
            {
                Console.WriteLine($"Palavra {i+1}: {Palavras[i]}");
            }

            var Textos_Paginas = new System.Text.StringBuilder();
            string[] Textos_Paginas_Separadas =  new string[Numero_Documentos]; //Vetor de string para armazenar textos de cada documento

            //Gera o caminho para os 20 arquivos
            //Abre todos os arquivos percorrendo t (t = 1:20) e acumula os textos em uma string unica chamada Textos_Paginas
            for(int t = 1; t <= Numero_Documentos; t++)
            {
                Caminho_Noticias = Caminho_Documentos;
                string c = t.ToString();
                Caminho_Noticias_Temp = String.Concat(Caminho_Noticias, $"/Noticia{c}.pdf");

                //Abre um arquivo PDF com o caminho de cada noticia
                PdfReader Arquivo_PDF = new PdfReader($"{Caminho_Noticias_Temp}");
                var Textos_Paginas_Temp = new System.Text.StringBuilder();
                
                //Laco para percorrer cada pagina de cada documento
                for(int i = 1; i <= Arquivo_PDF.NumberOfPages; i ++)
                {
                    //Obtem a informação da pagina inteira e une a string ja existinte. Ao mesmo tempo, remove letras maiusculas
                    string String_Aux = (PdfTextExtractor.GetTextFromPage(Arquivo_PDF, i)).ToLower();
                    //As 15 primeiras palavras palavras de cada página foram definidas como cabeçalho, portanto, devem ser retiradas
                    Textos_Paginas_Temp.Append(String_Aux.Remove(0, 104));
                }
                
                //Preenche o vetor com o texto na posicao referente ao numero do documento
                Textos_Paginas_Separadas[t - 1] = (Textos_Paginas_Temp).ToString(); 
            }

            //String com stop words
            string[] StopWords = new string[116] {"de", "o", "a", "que", "e", "do", "da", "em", "um", "para", "com", "uma",
            "os", "no", "se", "na", "por", "mais", "as", "dos", "por", "como", "mas", "foi", "ao", "ele", "das", "tem",
            "seu", "sua", "ou", "ser", "quando", "muito", "ha","nos", "ja", "esta", "eu", "tambem", "so", "pelo",
             "pela", "ate", "isso", "ela", "entre", "era", "depois", "sem", "mesmo", "aos", "ter", "seus", "quem", "nas", 
             "me", "esse", "eles", "estao", "voce", "tinha", "foram", "essa", "num", "nem", "suas", "meu", "minha", "numa",
             "pelos", "elas", "havia", "seja", "qual", "sera", "tenho", "lhes", "deles", "essas", "esses", "pelas", "este", "fosse", "dele",
              "tu", "te", "voces", "vos", "lhes", "meus", "minhas", "teu", "tua", "teus", "tuas", "nossos", "nossas", "dela", "delas", 
              "nao", "esta", "estes", "estas", "aquele", "aquela", "aqueles", "aquelas", "isto", "aquilo", "estou", "estamos", "estao", "estive",
              "esteve", "estivemos"};
             
            string[,] Matriz_Palavras_Paginas = new string[Numero_Documentos, 600];

            for(int doc = 1; doc <=Numero_Documentos; doc ++)
            {
                    //----------Separa as palavras e substitui caracteres indesejados
                    //Define separadores para as palavras
                    char[] Separadores = new char[] {'(', '.', ',', ':',' ', ')', '\n','-', '/'};
                    //Separa Palavras por pagina
                    string[] Palavras_Separadas_Paginas = (Textos_Paginas_Separadas[doc - 1]).Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                    //Loop para remover acentos, cedilha, etc
                    for(int s = 0; s < Palavras_Separadas_Paginas.Length; s ++)
                    {
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('ã', 'a');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('á', 'a');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('à', 'a');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('ê', 'e');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('é', 'e');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('í', 'i');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('ó', 'o');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('õ', 'o');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('ú', 'u');
                                Palavras_Separadas_Paginas[s] = Palavras_Separadas_Paginas[s].Replace('ç', 'c');
                    }
                    
                
                //----------Remoção dos stop words e dos colchetes []
                var StringBuilder_Auxiliar = new System.Text.StringBuilder();
                for(int Sw = 0; Sw < StopWords.Length; Sw ++)
                {
                    for(int P = 0; P < Palavras_Separadas_Paginas.Length; P ++)
                    {
                        if(Palavras_Separadas_Paginas[P].Equals(StopWords[Sw]) || Palavras_Separadas_Paginas[P].Equals("[") || Palavras_Separadas_Paginas[P].Equals("]"))
                        {
                            Palavras_Separadas_Paginas[P] = " ";
                        }
                    }
                }

                for(int P = 0; P < Palavras_Separadas_Paginas.Length; P ++)
                {
                    if(!Palavras_Separadas_Paginas[P].Equals(" "))
                    {
                        StringBuilder_Auxiliar.Append($"{Palavras_Separadas_Paginas[P]}\n");
                    }
                        
                }
                string[] String_Auxiliar = (StringBuilder_Auxiliar.ToString()).Split('\n');
        
                for(int j = 0; j < String_Auxiliar.Length; j++)
                {
                    //Preenche matriz de palavras por pagina
                    Matriz_Palavras_Paginas[doc - 1, j] = String_Auxiliar[j];
                    
                }

            }

            //------------------------OBTENCAO DOS TERMOS
            string[,] Termos = new string[Numero_Documentos,500];
            int[,] Quantidade_Cada_Termo = new int[Numero_Documentos,500];
            int[] Quantidade_De_Termos = new int[Numero_Documentos];
            string Termo;
            int p = 0;

            for(int d = 1; d <= Numero_Documentos; d++)
            { 
                do{
                    if(!String.Equals(Matriz_Palavras_Paginas[d-1, p], " "))                            //Se nao for um espaco
                    {
                         
                         Quantidade_De_Termos[d-1] ++;
                         Termo = Matriz_Palavras_Paginas[d-1, p];                                       //Atribui o termo para pesquisar
                         Termos[d - 1, Quantidade_De_Termos[d-1]-1] = Termo;                            //Coloca o termo no vetor de termos

                         for(int p2 = 0; p2 < 300; p2 ++)
                         {
                            //Console.WriteLine($"{Matriz_Palavras_Paginas[d - 1, p2]}");
                            if(String.Equals(Termos[d-1, Quantidade_De_Termos[d-1] - 1], Matriz_Palavras_Paginas[d - 1, p2]))
                            {
                                Quantidade_Cada_Termo[d-1, Quantidade_De_Termos[d-1] - 1] ++;
                                Matriz_Palavras_Paginas[d-1, p2] = " ";                                //Susbtitui o termo buscado por um espaco
                            }
                         }
                    }
                    p++;
                }while(p < 300);
            p = 0;
            }

            

            int[] acc = new int[Numero_Documentos];
            for(int u = 1; u <= Numero_Documentos; u++)
            {
                Console.WriteLine($"\n\nTERMOS E QUANTIDADES DO ARQUIVO {u}:");
                Console.WriteLine($"\nQuantidade de termos: {Quantidade_De_Termos[u-1]}.");

                    for(int y = 0; y < Quantidade_De_Termos[u-1] - 2; y ++)
                    {
                        Console.Write($"Termo '{Termos[u-1,y]}': Quantidade: {Quantidade_Cada_Termo[u - 1, y]}.  ");
                    }
            }

            //Para cada documento percorre-se as palavras a serem buscadas e as palavras do vocabulario
            int[] Similaridade_Documentos = new int[Numero_Documentos];
            for(int Doc = 1; Doc <= Numero_Documentos; Doc ++)                  //Percorre os documentos
            {
                Similaridade_Documentos[Doc - 1] = 0;
                for(int Palav = 0; Palav < Quantidade_Palavras; Palav++)        //Percorre as palavras a serem buscadas
                {
                    for(int Vocab = 0; Vocab < Quantidade_De_Termos[Doc-1] -2; Vocab ++ )
                    {
                            if(String.Equals(Palavras[Palav], Termos[Doc - 1, Vocab]))
                            {
                                Similaridade_Documentos[Doc - 1] += 1 * Quantidade_Cada_Termo[Doc-1, Vocab];
                            }
                    }
                } 
            }

            for(int y = 1; y <= Numero_Documentos; y ++)
            {
                 Console.WriteLine($"Similaridade do Arquivo {y}: {Similaridade_Documentos[y-1]}");
            }
        }
    }
}
