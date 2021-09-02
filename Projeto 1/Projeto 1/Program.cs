using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;


/*
Nome: Arthur Dilly Escher
Data 02/09/2021

Curso de Extensão Mineração de Texto em C# - Projeto 1
*/
class Program
{
    static void Main(string[] args)
    {
            string Caminho_Arquivo;

            Console.WriteLine(@"Digite o caminho do arquivo a ser usado:");
            Caminho_Arquivo = Console.ReadLine();

            Console.WriteLine(Caminho_Arquivo);

            PdfReader Arquivo_PDF = new PdfReader($"{Caminho_Arquivo}");

            int Pages = Arquivo_PDF.NumberOfPages;

            Console.WriteLine($"Numero de paginas: {Pages}");
            Console.WriteLine("Informe as palavras a serem buscadas (operadores AND ou OR)");

            string Words = Console.ReadLine();

            int[] Vet_Pos_Operador = new int[50];
            char[] Vet_Tipo_Operador = new char[50];
            int[] Vet_Pos_Palavras = new int[50];
            int[] Vet_Pos_Parenteses = new int[50];

            bool Sem_Operador = true;
            //bool Parenteses = false;
            int Qnt_Operadores = 0;
            int Qnt_Palavras = 0;                                              //Armazena o numero de operadores AND e OR
            //int Qnt_Parenteses = 0;

            /*
            for(int i = 0; i < Words.Length; i++)
            {
                if(Words[i] == '(' || Words[i] == ')')
                {
                    Vet_Pos_Parenteses[Qnt_Parenteses] = i;
                    Console.WriteLine($"Posicao parenteses: {Vet_Pos_Parenteses[Qnt_Parenteses]}");
                    Qnt_Parenteses ++;
                    Parenteses = true;
                }

            }
            */
            for(int i = 0; i < Words.Length; i ++)
            {
                if(Words[i] == 'A' && Words[i+1] == 'N' && Words[i+1] == 'N')
                {
                    Vet_Pos_Operador[Qnt_Operadores] = i;                     //Posicao do operador X na posicao X respectiva 
                    Vet_Pos_Palavras[Qnt_Operadores + 1] = i + 4;             //Armazena a posicao inicial das palavras
                    Vet_Tipo_Operador[Qnt_Operadores] = 'A'; 
                    Qnt_Operadores ++;                                        //Armazena o numero de operadores
                    Sem_Operador = false;
                }else{

                    if(Words[i] == 'O' && Words[i + 1] == 'R')
                    {
                         Vet_Pos_Operador[Qnt_Operadores] = i;                //Posicao do operador X na posicao X respectiva 
                         Vet_Pos_Palavras[Qnt_Operadores + 1] = i + 3;        //Armazena a posicao inicial das palavras
                         Vet_Tipo_Operador[Qnt_Operadores] = 'O'; 
                         Qnt_Operadores ++;                                   //Armazena o numero de operadores
                         Sem_Operador = false;
                    }
                }
            }


            Qnt_Palavras = Qnt_Operadores + 1;

            string[] Palavras = new string[Qnt_Palavras];                      //Cria string para armazenar as palavras a serem buscadas

            /*
            for(int t = 0; t < Qnt_Operadores; t ++)
            {
                Console.WriteLine($"Posição do operador: {Vet_Pos_Operador[t]}; Tipo do operador: {Vet_Tipo_Operador[t]}..");
            }
            for(int t = 0; t < Qnt_Operadores + 1; t ++)
            {
                Console.WriteLine($"Posição das palavras: {Vet_Pos_Palavras[t]}..");
            }
            */

            for(int p = 0; p < Qnt_Operadores; p ++)
            {
                Palavras[p] = Words.Substring(Vet_Pos_Palavras[p], ((Vet_Pos_Operador[p] - 1) - (Vet_Pos_Palavras[p])));
                Console.WriteLine($"Palavra {p + 1}: {Palavras[p]}");
            }

            //Preenche vetor "Palavras" com as palavras a serem buscadas
            //Posicoes dadas a partir da localização dos operadores AND a/ou OR
            Palavras[Qnt_Palavras - 1] = Words.Substring(Vet_Pos_Palavras[Qnt_Palavras - 1], (Words.Length- (Vet_Pos_Palavras[Qnt_Palavras-1])));

            Console.WriteLine($"Palavra {Qnt_Palavras}: {Palavras[Qnt_Palavras - 1]}");

            for(int s = 0; s < Palavras.Length; s ++)
                {
                    Palavras[s] = Palavras[s].Replace('ã', 'a');
                    Palavras[s] = Palavras[s].Replace('ç', 'c');
                    Palavras[s] = Palavras[s].Replace('á', 'a');
                    Palavras[s] = Palavras[s].Replace('ó', 'o');
                    Palavras[s] = Palavras[s].Replace('é', 'e');
                    Palavras[s] = Palavras[s].Replace('à', 'a');
                    Palavras[s] = Palavras[s].Replace('ú', 'u');
                    Palavras[s] = Palavras[s].Replace('ê', 'e');
                }
            
            //------------------Fim obtenção palavras------------------
            
            //Laço for para obter as informações das páginas

            var Texto_Arquivo_TXT = new System.Text.StringBuilder();

            //Arquivo txt para escrever as informações de historico
            string Caminho_Arquivo_TXT = @"C:\Users\Public\Documents\Mineracao_C#\Historio_Buscas.txt";
            using System.IO.StreamWriter Arquivo_TXT = new StreamWriter(Caminho_Arquivo_TXT, append : true);

            int[] Qnt_Cada_Palavra = new int[Qnt_Palavras]; 

            //PROCURA DE PALAVRAS
            for(int i = 1; i <= Arquivo_PDF.NumberOfPages; i ++)
            {
                string StringAux_Pag = (PdfTextExtractor.GetTextFromPage(Arquivo_PDF, i)).ToLower();

                char[] Separadores = new char[] {'(', '.', ',', ':', ' ', ')'};
                string[] Palavras_Separadas = StringAux_Pag.Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                
                for(int s = 0; s < Palavras_Separadas.Length; s ++)
                {
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('ã', 'a');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('ç', 'c');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('á', 'a');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('ó', 'o');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('é', 'e');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('à', 'a');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('ú', 'u');
                    Palavras_Separadas[s] = Palavras_Separadas[s].Replace('ê', 'e');
                }

                for(int p = 0; p < Palavras.Length; p ++) //Para cada palavra no vetor de Palavras a serem buscadas
                {
                    foreach(string linha in Palavras_Separadas)
                    {
                        if(linha.Contains($"{Palavras[p]}") && linha.Length == Palavras[p].Length)  //Se a linha conter XXXXX ou XXXXX
                        {
                            Qnt_Cada_Palavra[p] ++;
                        }
                    }
                }
            }

            char c = (char)92;                          
            int Pos = 0;

            //Escreve no documento .txt
            for(int i = 0; i < Caminho_Arquivo.Length; i ++)
            {
                if(Caminho_Arquivo[i] == c)
                {
                    Pos = i;
                }
            }

            //Salva o nome do arquivo e coloca no stringbuilder
            string Nome_Arquivo = Caminho_Arquivo.Substring(39, (Caminho_Arquivo.Length - (Pos + 1)));
            Texto_Arquivo_TXT.Append($"--------------------------------------------------\n\n");
            Texto_Arquivo_TXT.Append($"Nome do arquivo: {Nome_Arquivo}\n\n");
            bool AND_Logic = true;
            bool OR_Logic = false;

            //Faz a logica referente ao operador
            if(Vet_Tipo_Operador[0] == 'A')                              //Se o operador for AND
            {
                foreach(int Quantidade in Qnt_Cada_Palavra)
                {
                    if(Quantidade == 0)
                    {
                        AND_Logic = false;                               //Se qualquer quantidade for igual a ZERO, a logica AND é falsa
                    }
                }
            }else{
                if(Vet_Tipo_Operador[0] == 'O')
                {
                    foreach(int Quantidade in Qnt_Cada_Palavra)
                    {
                        if(Quantidade != 0)
                        {
                            OR_Logic = true;                            //Se qualquer quantidade for diferente de ZERO, a logica OR é verdadeira
                        }
                    }
                }
            }

            //Console.WriteLine(OR_Logic);
            //Console.WriteLine(Sem_Operador);
            //Escreve no arquivo txt baseado no resultado das operações
            //
            if(Sem_Operador == true)
            {
                Texto_Arquivo_TXT.Append($"Palavra '{Palavras[0]}': Quantidade = {Qnt_Cada_Palavra[0]} \n");
            }
            else{
                    if(AND_Logic == false && Vet_Tipo_Operador[0] == 'A')
                    {
                        Texto_Arquivo_TXT.Append($"As palavras procuradas não foram encontradas no texto.\n\n");

                    }else{
                        if(AND_Logic == true && Vet_Tipo_Operador[0] == 'A')
                        {
                            Texto_Arquivo_TXT.Append($"As palavras procuradas foram encontradas no texto.\n\n");
                            for(int w = 0; w < Qnt_Palavras; w++)
                            {
                                //Console.WriteLine($"Palavra '{Palavras[w]}': Quantidade = {Qnt_Cada_Palavra[w]}");
                                Texto_Arquivo_TXT.Append($"Palavra '{Palavras[w]}': Quantidade = {Qnt_Cada_Palavra[w]} \n");
                            } 
                        }          
                    }

                    if(OR_Logic == false && Vet_Tipo_Operador[0] == 'O')
                    {
                        Texto_Arquivo_TXT.Append($"Nenhuma das palavras procuradas foram encontradas no texto.\n\n");
                    }else{
                            if(OR_Logic == true && Vet_Tipo_Operador[0] == 'O')
                            {
                                for(int w = 0; w < Qnt_Palavras; w++)
                                {
                                    //Console.WriteLine($"Palavra '{Palavras[w]}': Quantidade = {Qnt_Cada_Palavra[w]}");
                                    Texto_Arquivo_TXT.Append($"Palavra '{Palavras[w]}': Quantidade = {Qnt_Cada_Palavra[w]} \n");
                            
                                }  
                            }
                    }
            }

            DateTime D = DateTime.Today; 
            string Data = D.ToShortDateString();

            Texto_Arquivo_TXT.Append($"\nString buscada: {Words} \n\nData: {Data}\n");
            
            Arquivo_TXT.WriteLine(Texto_Arquivo_TXT);
            Console.WriteLine($"Os dados foram escritos no arquivo: {Caminho_Arquivo_TXT}");
  
    }
}

