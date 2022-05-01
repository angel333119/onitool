using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Onitool
{
    public partial class Form1 : Form
    {
        uint bigendian(uint le)
        {
            //Converte um valor de 16Bits de Big endian para Little endian
            uint be = (uint)((byte)(le >> 8) | ((byte)le << 8));
            return be;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //extrator
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo Onimusha PS2|*.RDT|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo RTD do jogo Onimusha de PS2...";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    //Verifica se é algum dos arquivos que não tem texto e se for manda a mensagem na tela e para o programa
                    string arquivo = Path.GetFileName(file);
                    var lista = new List<string>() { "room001.rdt","room004.rdt","room006.rdt","room012.rdt","room100.rdt","room102.rdt","room103.rdt","room106.rdt","room109.rdt","room111.rdt","room112.rdt","room113.rdt","room114.rdt","room115.rdt","room119.rdt","room120.rdt","room122.rdt","room123.rdt","room124.rdt","room200.rdt","room202.rdt","room203.rdt","room204.rdt","room205.rdt","room206.rdt","room208.rdt","room209.rdt","room210.rdt","room211.rdt","room212.rdt","room213.rdt","room215.rdt","room216.rdt","room217.rdt","room218.rdt","room219.rdt","room220.rdt","room221.rdt","room223.rdt","room224.rdt","room225.rdt","room227.rdt","room228.rdt","room229.rdt","room231.rdt","room232.rdt","room233.rdt","room234.rdt","room237.rdt","room240.rdt","room241.rdt","room242.rdt","room300.rdt","room301.rdt","room302.rdt","room303.rdt","room304.rdt","room305.rdt","room306.rdt","room307.rdt","room309.rdt","room310.rdt","room403.rdt","room405.rdt","room500.rdt","room502.rdt","room504.rdt","room506.rdt","room509.rdt","room510.rdt","room511.rdt","room512.rdt","room513.rdt","room514.rdt","room515.rdt","room523.rdt","room530.rdt","room531.rdt","room532.rdt"};
                    if (lista.Contains(arquivo))
                    {
                        MessageBox.Show("O arquivo " + arquivo + " não é suportado", "AVISO!");
                        return;
                    }

                    using (FileStream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);
                        BinaryWriter bw = new BinaryWriter(stream);

                        int magic = br.ReadInt32(); // Read magic 0x4B4E494C - Coloquei atoua mesmo

                        br.BaseStream.Seek(0x84, SeekOrigin.Begin); // Vai pro offset 0x84

                        int localponteiro = br.ReadInt32(); // Lê o offset de onde inicia o arquivo 11 que é onde está os textos e e consequentemente o primeiro ponteiro dos textos

                        int finalarquivo = br.ReadInt32(); // Lê o tamnho do arquivo

                        int terminoarquivo = localponteiro + finalarquivo - 1;

                        br.BaseStream.Seek(localponteiro, SeekOrigin.Begin); // Volta pro começo dos ponteiros

                        int primeiroponteiro = br.ReadInt16();

                        br.BaseStream.Seek(localponteiro, SeekOrigin.Begin); // Volta pro começo dos ponteiros

                        int quantidadeponteiros = primeiroponteiro / 2;

                        int[] ponteiros = new int[quantidadeponteiros]; // Inicia o array que vai guardar o valor de cada ponteiro

                        int ponteiro = 0; // Variavel que usarei para definir a localização do ponteiro no array

                        for (ponteiro = 0; ponteiro < quantidadeponteiros; ponteiro++) // Faz o loop com cada ponteiro
                        {
                            int ponteiroatual = br.ReadInt16(); // Lê o ponteiro

                            ponteiros[ponteiro] = ponteiroatual; // O array recebe o valor de cada ponteiro
                        }

                        int comparador = 0;
                        string todosOsTextos = "";
                        string convertido;

                        // Extração dos textos
                        for (ponteiro = 0; ponteiro < quantidadeponteiros; ponteiro++) // Faz o loop com cada ponteiro
                        {
                            if (ponteiro + 1 < quantidadeponteiros)
                            {
                                int totalbytes = ponteiros[ponteiro + 1] - ponteiros[ponteiro]; // Faz a conta para saber quantos bytes de texto será lido

                                br.BaseStream.Seek(ponteiros[ponteiro] + localponteiro, SeekOrigin.Begin); // Vai pro texto

                                for (int i = 1; i <= totalbytes; i++)
                                {
                                    //Lê um byte do texto
                                    comparador = br.ReadByte();
                                    //compara se o byte é a endstring
                                    //se for, o programa cria uma nova linha
                                    //se não continua pra proxima letra

                                    if (comparador == 0)
                                    {
                                        convertido = ' '.ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xF2)
                                    {
                                        convertido = "<ql>".ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xFA)
                                    {
                                        i++;
                                        comparador = br.ReadByte();
                                        //Começa a conversão dos caracteres
                                        convertido = tabela.Converterascii(comparador);
                                        todosOsTextos += convertido;
                                    }
                                    else
                                    {
                                        //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                                        convertido = ("<" + comparador.ToString("X2") + ">");
                                        todosOsTextos += convertido;
                                    }
                                }
                                todosOsTextos += "<end>\r\n";
                            }
                            else
                            {
                                br.BaseStream.Seek(ponteiros[ponteiro] + localponteiro, SeekOrigin.Begin); // Vai pro texto

                                int totalbytes = terminoarquivo - (ponteiros[ponteiro] + localponteiro) + 1;

                                for (int i = 1; i <= totalbytes; i++)
                                {
                                    //Lê um byte do texto
                                    comparador = br.ReadByte();
                                    //compara se o byte é a endstring
                                    //se for, o programa cria uma nova linha
                                    //se não continua pra proxima letra

                                    if (comparador == 0)
                                    {
                                        convertido = ' '.ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xF2)
                                    {
                                        convertido = "<ql>".ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xFA)
                                    {
                                        i++;
                                        comparador = br.ReadByte();
                                        //Começa a conversão dos caracteres
                                        convertido = tabela.Converterascii(comparador);
                                        todosOsTextos += convertido;
                                    }
                                    else
                                    {
                                        //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                                        convertido = ("<" + comparador.ToString("X2") + ">");
                                        todosOsTextos += convertido;
                                    }
                                }
                                todosOsTextos += "<end>\r\n";
                            }
                        }
                        //aqui já terminou de ler todos os textos, escreve o TXT com o texto dumpado
                        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)) + ".txt", todosOsTextos);
                        todosOsTextos = "";
                    }
                }
                MessageBox.Show("Texto extraido com sucesso!", "AVISO!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //inserçor
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo Onimusha PS2|*.RDT|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo RTD do jogo Onimusha de PS2...";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String dump in openFileDialog1.FileNames)
                {
                    string pasta = ""; // Inicia a variavel pasta onde ficará guardado os arquivos extraidos

                    FileInfo file = new FileInfo(Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)) + ".txt"); // Verifica se o txt existe

                    if (file.Exists)
                    {
                        //A variavel arquivoaserescrito recebe o nome do aquivo sem a extensão
                        string arquivoaserescrito = Path.GetFileNameWithoutExtension(dump);

                        //Cria uma pasta temporaria com o nome do arquivo aberto para salvar os arquivos dentro
                        Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)));

                        //A variavel pasta recebe o caminho de onde a pasta foi criada
                        pasta = (Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)));

                        using (FileStream stream = File.Open(dump, FileMode.Open))
                        {
                            BinaryReader br = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            int magic = br.ReadInt32(); // Read magic

                            int quantidadearquivos = br.ReadInt32();

                            // Extrai o RDT em vários arquivos pra inserir o texto

                            for (int i = 1; i <= quantidadearquivos; i++)
                            {
                                br.BaseStream.Seek(0x0C * i, SeekOrigin.Begin);

                                int offsetarquivo = br.ReadInt32();

                                int tamanhoarquivo = br.ReadInt32();

                                br.BaseStream.Seek(offsetarquivo, SeekOrigin.Begin); //Vai pro offset do arquivo

                                byte[] arquivoaserextraido = br.ReadBytes(tamanhoarquivo); //Lê o arquivo todo

                                //escreve o arquivo dumpado, dentro da pasta criada
                                File.WriteAllBytes(Path.Combine(pasta, arquivoaserescrito + "_" + i), arquivoaserextraido);
                            }
                        }

                        // Abre o arquivo pra inserir o texto
                        // O texto deve ser inserido sempre no arquivo número 11

                        using (FileStream stream = File.Open(Path.Combine(pasta, arquivoaserescrito + "_11"), FileMode.Open))
                        {
                            BinaryReader br = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            string txt = File.ReadAllText(Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)) + ".txt");

                            string[] texto = txt.Split(new string[] { "<end>\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                            br.BaseStream.Seek(0, SeekOrigin.Begin);

                            int ponteiro = br.ReadInt16();

                            int convertido = ' ';

                            int proximoponteiro = 0;

                            for (int dialogo = 0; dialogo < texto.Length; dialogo++)
                            {
                                bw.BaseStream.Seek(proximoponteiro, SeekOrigin.Begin);

                                int novoponteiro;

                                novoponteiro = ponteiro;

                                bw.Write((ushort)novoponteiro); //unit se for 32bits e ushort se for 16 bits

                                for (int c = 0; c < texto[dialogo].Length; c++)
                                {
                                    bool doisbytes = false;

                                    bw.BaseStream.Seek(ponteiro, SeekOrigin.Begin);

                                    char caractere = texto[dialogo][c];

                                    if (caractere == '<')
                                    {
                                        string outputstring = "";
                                        string entresinal = texto[dialogo];
                                        int inicial = c;
                                        int final = entresinal.IndexOf('>', c + 1);
                                        outputstring = entresinal.Substring(inicial + 1, final - inicial - 1);

                                        if (outputstring == "ql") //Se for \n, temos uma sequencia \r\n, pula próximo caractere.
                                        {
                                            c += 3;
                                            convertido = 0xF2;
                                            doisbytes = false;
                                        }
                                        else
                                        {
                                            ushort numero = Convert.ToUInt16(outputstring, 16);
                                            convertido = numero;
                                            c += 3;
                                            doisbytes = false;
                                        }
                                    }
                                    else if (caractere == ' ')
                                    {
                                        convertido = 0x00;
                                        doisbytes = false;
                                    }
                                    else
                                    {
                                        convertido = tabela.hextoascii(caractere);
                                        ponteiro += 1;
                                        doisbytes = true;
                                    }

                                    if (doisbytes == true)
                                    {
                                        bw.Write(bigendian((uint)convertido));
                                        doisbytes = false;
                                    }
                                    else
                                    {
                                        bw.Write((uint)convertido);
                                        doisbytes = false;
                                    }
                                    ponteiro += 1;
                                }
                                proximoponteiro += 2;//Se o ponteiro for 32Bits coloque 4, se for 16Bits coloque 2
                            }
                        }

                        using (FileStream stream = File.Open(dump, FileMode.Open))
                        {
                            BinaryReader bro = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            int magic = bro.ReadInt32(); // Read magic

                            int quantidadearquivos = bro.ReadInt32(); // Lê a a quantidade de arquivos

                            bw.BaseStream.Seek(0x0C, SeekOrigin.Begin);

                            int primeiroarquivo = bro.ReadInt32();

                            int offsetatual = primeiroarquivo;

                            // Inicia o array que vai guardar o nome de cada arquivo.
                            // Ele tem o +1 por começar em 0, e a variavel começar em 1, então precisa de um a mais
                            string[] nomearquivos = new string[quantidadearquivos + 1];

                            for (int i = 1; i <= quantidadearquivos; i++) // Faz o loop com cada arquivo
                            {
                                nomearquivos[i] = Path.Combine(pasta, arquivoaserescrito + "_" + i);
                            }

                            // Verifica se o arquivo já existe, apaga se existir ou cria se não existir
                            if (File.Exists(dump + "_novo"))
                            {
                                File.Delete(dump + "_novo");
                            }
                            else
                            {
                                // Cria o arquivo    
                                using (FileStream fs = File.Create(dump + "_novo"))
                                {
                                    BinaryReader brn = new BinaryReader(fs);
                                    BinaryWriter bwn = new BinaryWriter(fs);

                                    bro.BaseStream.Seek(0x0C, SeekOrigin.Begin);

                                    int tamanhocabecalho = bro.ReadInt32();

                                    bro.BaseStream.Seek(0, SeekOrigin.Begin);

                                    byte[] cabecalho = bro.ReadBytes(tamanhocabecalho); //Lê o cabeçalho do arquivo original

                                    bwn.Write(cabecalho); // Escreve o cabeçalho no arquivo novo

                                    for (int j = 1; j <= quantidadearquivos; j++) // Faz o loop com cada arquivo
                                    {

                                        FileInfo fi = new FileInfo(nomearquivos[j]);  //informa o tamanho do arquivo em bytes
                                        long tamanhoarquivonovo = (fi.Length); //tamanho arquivo novo recebe o valor do tamanho

                                        int tamanhonovo = Convert.ToInt32(tamanhoarquivonovo);

                                        byte[] conteudoarquivo = File.ReadAllBytes(nomearquivos[j]); //Lê todo o arquivo para a matriz conteudo do arquivo

                                        int resultado = Convert.ToInt32(tamanhoarquivonovo) / 16;

                                        int resultado2 = Convert.ToInt32(tamanhoarquivonovo) % 16;

                                        int blocos = resultado;

                                        if (resultado2 != 0)
                                        {
                                            blocos = resultado + 1;
                                        }

                                        int tamanhocompadding = blocos * 16;

                                        bwn.BaseStream.Seek(0x0C * j, SeekOrigin.Begin);

                                        bwn.Write(offsetatual);

                                        bwn.Write(tamanhonovo);

                                        bwn.BaseStream.Seek(offsetatual, SeekOrigin.Begin);
                                                                                
                                        bwn.Write(conteudoarquivo); //Escreve o arquivo

                                        offsetatual += tamanhocompadding;
                                    }
                                }
                            }
                        }
                        Directory.Delete(pasta, true);
                    }
                    else
                    {
                        MessageBox.Show("Arquivo TXT não encontrado!", "AVISO!");

                    }
                }
                MessageBox.Show("Texto inserido e arquivo(s) remontado(s)!", "AVISO!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Extração texto SLUS
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo Onimusha PS2|SLUS_200.18|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo LDT do jogo Onimusha de PS2...";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog1.FileName;

                using (FileStream stream = File.Open(file, FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(stream);
                    BinaryWriter bw = new BinaryWriter(stream);

                    int magic = br.ReadInt32(); // Read magic

                    //Os ponteiros estão depois do texto

                    int[] quantidadetexto = new int[5];

                    quantidadetexto[1] = 161;
                    quantidadetexto[2] = 20;
                    quantidadetexto[3] = 105;
                    quantidadetexto[4] = 28;


                    int[] localtextos = new int[5];

                    localtextos[1] = 0x1014E0;
                    localtextos[2] = 0x1021E0;
                    localtextos[3] = 0x102A50;
                    localtextos[4] = 0x104340;


                    int[] localponteiro = new int[5];

                    localponteiro[1] = 0x102090;
                    localponteiro[2] = 0x102A20;
                    localponteiro[3] = 0x104260;
                    localponteiro[4] = 0x104430;

                    if (magic == 0x464C457F) // .ELF
                    {
                        for (int a = 1; a < 5; a++)
                        {

                            br.BaseStream.Seek(localponteiro[a], SeekOrigin.Begin);

                            int[] ponteiros = new int[quantidadetexto[a]]; // Inicia o array que vai guardar o valor de cada ponteiro

                            int ponteiro = 0; // Variavel que usarei para definir a localização do ponteiro no array

                            for (ponteiro = 0; ponteiro < quantidadetexto[a]; ponteiro++) // Faz o loop com cada ponteiro
                            {
                                int ponteiroatual = br.ReadInt16(); // Lê o ponteiro

                                ponteiros[ponteiro] = ponteiroatual; // O array recebe o valor de cada ponteiro

                            }

                            int comparador = 0;
                            string todosOsTextos = "";
                            string convertido;

                            // Extração dos textos

                            for (ponteiro = 0; ponteiro < quantidadetexto[a]; ponteiro++) // Faz o loop com cada ponteiro
                            {
                                if (ponteiro + 1 < quantidadetexto[a])
                                {
                                    int totalbytes = ponteiros[ponteiro + 1] - ponteiros[ponteiro]; // Faz a conta para saber quantos bytes de texto será lido

                                    br.BaseStream.Seek(localtextos[a] + ponteiros[ponteiro], SeekOrigin.Begin); // Vai pro texto

                                    for (int i = 1; i <= totalbytes; i++)
                                    {
                                        //Lê um byte do texto
                                        comparador = br.ReadByte();
                                        //compara se o byte é a endstring
                                        //se for, o programa cria uma nova linha
                                        //se não continua pra proxima letra

                                        if (comparador == 0)
                                        {
                                            convertido = ' '.ToString();
                                            todosOsTextos += convertido;
                                        }
                                        else if (comparador == 0xF2)
                                        {
                                            convertido = "<ql>".ToString();
                                            todosOsTextos += convertido;
                                        }
                                        else if (comparador == 0xFA)
                                        {
                                            i++;
                                            comparador = br.ReadByte();
                                            //Começa a conversão dos caracteres
                                            convertido = tabela.Converterascii(comparador);
                                            todosOsTextos += convertido;
                                        }
                                        else
                                        {
                                            //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                                            convertido = ("<" + comparador.ToString("X2") + ">");
                                            todosOsTextos += convertido;
                                        }
                                    }
                                    todosOsTextos += "<end>\r\n";
                                }
                                else
                                {
                                    br.BaseStream.Seek(localtextos[a] + ponteiros[ponteiro], SeekOrigin.Begin); // Vai pro texto

                                    //Inicia a variavel que vai verificar se o texto acabou ou não
                                    bool acabouotexto = false;

                                    //Equanto não acabar o texto ele vai repetindo
                                    while (acabouotexto == false)
                                    {
                                        //Lê um byte do texto
                                        comparador = br.ReadByte();
                                        //compara se o byte é a endstring
                                        //se for, o programa cria uma nova linha
                                        //se não continua pra proxima letra

                                        if (comparador == 0xF7)
                                        {
                                            comparador = br.ReadByte();

                                            if (comparador == 0)
                                            {
                                                //Quando chegar em uma endstring ele retorna como o texto tendo acabado (acabouotexto = verdadeiro)
                                                acabouotexto = true;
                                                todosOsTextos += "<F7><end>\r\n";
                                            }
                                            else
                                            {
                                                todosOsTextos += "<F7>";
                                            }
                                        }
                                        else if (comparador == 0xF0)
                                        {
                                            comparador = br.ReadByte();

                                            if (comparador == 0)
                                            {
                                                //Quando chegar em uma endstring ele retorna como o texto tendo acabado (acabouotexto = verdadeiro)
                                                acabouotexto = true;
                                                todosOsTextos += "<F0> <end>\r\n";
                                            }
                                            else
                                            {
                                                todosOsTextos += "<F0>";
                                            }
                                        }
                                        else if (comparador == 0)
                                        {
                                            convertido = ' '.ToString();
                                            todosOsTextos += convertido;
                                        }
                                        else if (comparador == 0xF2)
                                        {
                                            convertido = "<ql>".ToString();
                                            todosOsTextos += convertido;
                                        }
                                        else if (comparador == 0xFA)
                                        {
                                            comparador = br.ReadByte();
                                            //Começa a conversão dos caracteres
                                            convertido = tabela.Converterascii(comparador);
                                            todosOsTextos += convertido;
                                        }
                                        else
                                        {
                                            //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                                            convertido = ("<" + comparador.ToString("X2") + ">");
                                            todosOsTextos += convertido;
                                        }
                                    }
                                }
                            }
                            //aqui já terminou de ler todos os textos, escreve o TXT com o texto dumpado
                            File.WriteAllText(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)) + "_" + a + ".txt", todosOsTextos);
                            todosOsTextos = "";
                        }
                    }
                }
                MessageBox.Show("Texto extraido com sucesso!", "AVISO!");
            }
            else

            {
                MessageBox.Show("Não é um arquivo SLUS válido!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Inserção texto SLUS
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo Onimusha PS2|SLUS_200.18|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo LDT do jogo Onimusha de PS2...";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog1.FileName;

                using (FileStream stream = File.Open(file, FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(stream);
                    BinaryWriter bw = new BinaryWriter(stream);

                    int magic = br.ReadInt32(); // Read magic

                    //Os ponteiros estão depois do texto

                    int[] quantidadetexto = new int[5];

                    quantidadetexto[1] = 161;
                    quantidadetexto[2] = 20;
                    quantidadetexto[3] = 105;
                    quantidadetexto[4] = 28;

                    int[] localtextos = new int[5];

                    localtextos[1] = 0x1014E0;
                    localtextos[2] = 0x1021E0;
                    localtextos[3] = 0x102A50;
                    localtextos[4] = 0x104340;


                    int[] localponteiro = new int[5];

                    localponteiro[1] = 0x102090;
                    localponteiro[2] = 0x102A20;
                    localponteiro[3] = 0x104260;
                    localponteiro[4] = 0x104430;

                    if (magic == 0x464C457F) // .elf
                    {
                        for (int a = 1; a < 5; a++)
                        {
                            Dictionary<string, int> dicionarioDeTextos = new Dictionary<string, int>();

                            string[] txt = new string[5];

                            txt[a] = File.ReadAllText(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)) + "_" + a + ".txt");

                            string[] texto = txt[a].Split(new string[] { "<end>\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                            br.BaseStream.Seek(localponteiro[a], SeekOrigin.Begin); //Vai pro endereço do primeiro ponteiro

                            int ponteiro = br.ReadInt16();

                            int convertido = ' ';

                            int proximoponteiro = 0; // Essa varável quarda a posição de onde o ponteiro vai ser escrito

                            for (int dialogo = 0; dialogo < texto.Length; dialogo++)
                            {
                                bw.BaseStream.Seek(proximoponteiro + localponteiro[a], SeekOrigin.Begin);

                                int novoponteiro;

                                novoponteiro = ponteiro;

                                if (dicionarioDeTextos.TryGetValue(texto[dialogo], out int ponteiroExistente))
                                {
                                    bw.Write((ushort)ponteiroExistente);
                                    proximoponteiro += 2;
                                    continue;
                                }

                                dicionarioDeTextos.Add(texto[dialogo], novoponteiro);

                                bw.Write((ushort)novoponteiro); //unit se for 32bits e ushort se for 16 bits

                                for (int c = 0; c < texto[dialogo].Length; c++)
                                {
                                    bool doisbytes = false;

                                    bw.BaseStream.Seek(ponteiro + localtextos[a], SeekOrigin.Begin);

                                    char caractere = texto[dialogo][c];

                                    if (caractere == '<')
                                    {
                                        string outputstring = "";

                                        string entresinal = texto[dialogo];
                                        int inicial = c;
                                        int final = entresinal.IndexOf('>', c + 1);
                                        outputstring = entresinal.Substring(inicial + 1, final - inicial - 1);

                                        if (outputstring == "ql")
                                        {
                                            c += 3;
                                            convertido = 0xF2;
                                            doisbytes = false;
                                        }
                                        else
                                        {
                                            ushort numero = Convert.ToUInt16(outputstring, 16);
                                            convertido = numero;
                                            c += 3;
                                            doisbytes = false;
                                        }
                                    }
                                    else if (caractere == ' ')
                                    {
                                        convertido = 0x00;
                                        doisbytes = false;
                                    }
                                    else
                                    {
                                        convertido = tabela.hextoascii(caractere);
                                        ponteiro += 1;
                                        doisbytes = true;
                                    }

                                    if (doisbytes == true)
                                    {
                                        bw.Write(bigendian((uint)convertido));
                                        doisbytes = false;
                                    }
                                    else
                                    {
                                        bw.Write((uint)convertido);
                                        doisbytes = false;
                                    }
                                    ponteiro += 1;
                                }
                                //Diz ao programa quantas bytes tem que andar pra escrever o proximo ponteiro no lugar certo
                                proximoponteiro += 2; //Se o ponteiro for 32Bits = 4, se for 16Bits = 2
                            }
                        }
                        MessageBox.Show("Texto inserido com sucesso!", "AVISO!");
                    }
                    else
                    {
                        MessageBox.Show("Não é um arquivo SLUS válido!");
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //extrator
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo Onimusha PS2|*.LDT|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo LTD do jogo Onimusha de PS2...";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    using (FileStream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);
                        BinaryWriter bw = new BinaryWriter(stream);

                        int enderecoarquivo = 0;

                        string arquivo = Path.GetFileName(file);

                        if (arquivo == "E_TITLE.LDT")
                        {
                            enderecoarquivo = 0x24;
                        }
                        else if (arquivo == "e_title.ldt")
                        {
                            enderecoarquivo = 0x24;
                        }
                        else if (arquivo == "CARDCK.LDT")
                        {
                            enderecoarquivo = 0x18;
                        }
                        else if (arquivo == "cardck.ldt")
                        {
                            enderecoarquivo = 0x18;
                        }
                        else
                        {
                            MessageBox.Show("Arquivo não suportado!", "AVISO!");
                            return;
                        }

                        int magic = br.ReadInt32(); // Read magic

                        br.BaseStream.Seek(enderecoarquivo, SeekOrigin.Begin);

                        int localponteiro = br.ReadInt32(); // Lê o offset do primeiro ponteiro

                        int finalarquivo = br.ReadInt32(); // Lê o tamnho do arquivo

                        int terminoarquivo = localponteiro + finalarquivo - 1;

                        br.BaseStream.Seek(localponteiro, SeekOrigin.Begin); // Volta pro começo dos ponteiros

                        int primeiroponteiro = br.ReadInt16();

                        br.BaseStream.Seek(localponteiro, SeekOrigin.Begin); // Volta pro começo dos ponteiros

                        int quantidadeponteiros = primeiroponteiro / 2;

                        int[] ponteiros = new int[quantidadeponteiros]; // Inicia o array que vai guardar o valor de cada ponteiro

                        int ponteiro = 0; // Variavel que usarei para definir a localização do ponteiro no array

                        for (ponteiro = 0; ponteiro < quantidadeponteiros; ponteiro++) // Faz o loop com cada ponteiro
                        {
                            int ponteiroatual = br.ReadInt16(); // Lê o ponteiro

                            ponteiros[ponteiro] = ponteiroatual; // O array recebe o valor de cada ponteiro

                        }

                        int comparador = 0;
                        string todosOsTextos = "";
                        string convertido;

                        // Extração dos textos
                        for (ponteiro = 0; ponteiro < quantidadeponteiros; ponteiro++) // Faz o loop com cada ponteiro
                        {
                            if (ponteiro + 1 < quantidadeponteiros)
                            {
                                int totalbytes = ponteiros[ponteiro + 1] - ponteiros[ponteiro]; // Faz a conta para saber quantos bytes de texto será lido

                                br.BaseStream.Seek(ponteiros[ponteiro] + localponteiro, SeekOrigin.Begin); // Vai pro texto

                                for (int i = 1; i <= totalbytes; i++)
                                {
                                    //Lê um byte do texto
                                    comparador = br.ReadByte();
                                    //compara se o byte é a endstring
                                    //se for, o programa cria uma nova linha
                                    //se não continua pra proxima letra

                                    if (comparador == 0)
                                    {
                                        convertido = ' '.ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xF2)
                                    {
                                        convertido = "<ql>".ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xFA)
                                    {
                                        i++;
                                        comparador = br.ReadByte();
                                        //Começa a conversão dos caracteres
                                        convertido = tabela.Converterascii(comparador);
                                        todosOsTextos += convertido;
                                    }
                                    else
                                    {
                                        //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                                        convertido = ("<" + comparador.ToString("X2") + ">");
                                        todosOsTextos += convertido;
                                    }
                                }
                                todosOsTextos += "<end>\r\n";
                            }
                            else
                            {
                                br.BaseStream.Seek(ponteiros[ponteiro] + localponteiro, SeekOrigin.Begin); // Vai pro texto

                                int totalbytes = terminoarquivo - (ponteiros[ponteiro] + localponteiro) + 1;

                                for (int i = 1; i <= totalbytes; i++)
                                {
                                    //Lê um byte do texto
                                    comparador = br.ReadByte();
                                    //compara se o byte é a endstring
                                    //se for, o programa cria uma nova linha
                                    //se não continua pra proxima letra

                                    if (comparador == 0)
                                    {
                                        convertido = ' '.ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xF2)
                                    {
                                        convertido = "<ql>".ToString();
                                        todosOsTextos += convertido;
                                    }
                                    else if (comparador == 0xFA)
                                    {
                                        i++;
                                        comparador = br.ReadByte();
                                        //Começa a conversão dos caracteres
                                        convertido = tabela.Converterascii(comparador);
                                        todosOsTextos += convertido;
                                    }
                                    else
                                    {
                                        //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                                        convertido = ("<" + comparador.ToString("X2") + ">");
                                        todosOsTextos += convertido;
                                    }
                                }
                                todosOsTextos += "<end>\r\n";
                            }
                        }
                        //aqui já terminou de ler todos os textos, escreve o TXT com o texto dumpado
                        File.WriteAllText(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)) + ".txt", todosOsTextos);
                        todosOsTextos = "";
                    }
                }
                MessageBox.Show("Texto extraido com sucesso!", "AVISO!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //inserçor
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo Onimusha PS2|*.LDT|Todos os arquivos (*.*)|*.*";
            openFileDialog1.Title = "Escolha um arquivo LTD do jogo Onimusha de PS2...";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String dump in openFileDialog1.FileNames)
                {
                    string arquivo = Path.GetFileName(dump);

                    string numeroarquivo;

                    if (arquivo == "E_TITLE.LDT")
                    {
                        numeroarquivo = "3";
                    }
                    else if (arquivo == "e_title.ldt")
                    {
                        numeroarquivo = "3";
                    }
                    else if (arquivo == "CARDCK.LDT")
                    {
                        numeroarquivo = "2";
                    }
                    else if (arquivo == "cardck.ldt")
                    {
                        numeroarquivo = "2";
                    }
                    else
                    {
                        MessageBox.Show("Arquivo não suportado!", "AVISO!");
                        return;
                    }

                    string pasta = ""; // Inicia a variavel pasta onde ficará guardado os arquivos extraidos

                    FileInfo file = new FileInfo(Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)) + ".txt"); // Verifica se o txt existe

                    if (file.Exists)
                    {
                        //A variavel arquivoaserescrito recebe o nome do aquivo sem a extensão
                        string arquivoaserescrito = Path.GetFileNameWithoutExtension(dump);

                        //Cria uma pasta temporaria com o nome do arquivo aberto para salvar os arquivos dentro
                        Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)));

                        //A variavel pasta recebe o caminho de onde a pasta foi criada
                        pasta = (Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)));

                        using (FileStream stream = File.Open(dump, FileMode.Open))
                        {
                            BinaryReader br = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            int magic = br.ReadInt32(); // Read magic 0x4B4E494C - coloquei atoua mesmo

                            int quantidadearquivos = br.ReadInt32();

                            // Extrai o LDT em vários arquivos pra inserir o texto

                            for (int i = 1; i <= quantidadearquivos; i++)
                            {
                                br.BaseStream.Seek(0x0C * i, SeekOrigin.Begin);

                                int offsetarquivo = br.ReadInt32();

                                int tamanhoarquivo = br.ReadInt32();

                                br.BaseStream.Seek(offsetarquivo, SeekOrigin.Begin); //Vai pro offset do arquivo

                                byte[] arquivoaserextraido = br.ReadBytes(tamanhoarquivo); //Lê o arquivo todo

                                //escreve o arquivo dumpado, dentro da pasta criada
                                File.WriteAllBytes(Path.Combine(pasta, arquivoaserescrito + "_" + i), arquivoaserextraido);
                            }
                        }

                        // Abre o arquivo pra inserir o texto
                        // O texto deve ser inserido sempre no arquivo número 3 se e_title ou 2 se cardck

                        using (FileStream stream = File.Open(Path.Combine(pasta, arquivoaserescrito + "_" + numeroarquivo), FileMode.Open))
                        {
                            BinaryReader br = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            string txt = File.ReadAllText(Path.Combine(Path.GetDirectoryName(dump), Path.GetFileNameWithoutExtension(dump)) + ".txt");

                            string[] texto = txt.Split(new string[] { "<end>\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                            br.BaseStream.Seek(0, SeekOrigin.Begin);

                            int ponteiro = br.ReadInt16();

                            int convertido = ' ';

                            int proximoponteiro = 0;

                            for (int dialogo = 0; dialogo < texto.Length; dialogo++)
                            {
                                bw.BaseStream.Seek(proximoponteiro, SeekOrigin.Begin);

                                int novoponteiro;

                                novoponteiro = ponteiro;

                                bw.Write((ushort)novoponteiro); //unit se for 32bits e ushort se for 16 bits

                                for (int c = 0; c < texto[dialogo].Length; c++)
                                {
                                    bool doisbytes = false;

                                    bw.BaseStream.Seek(ponteiro, SeekOrigin.Begin);

                                    char caractere = texto[dialogo][c];

                                    if (caractere == '<')
                                    {
                                        string outputstring = "";
                                        string entresinal = texto[dialogo];
                                        int inicial = c;
                                        int final = entresinal.IndexOf('>', c + 1);
                                        outputstring = entresinal.Substring(inicial + 1, final - inicial - 1);

                                        if (outputstring == "ql")
                                        {
                                            c += 3;
                                            convertido = 0xF2;
                                            doisbytes = false;
                                        }
                                        else
                                        {
                                            ushort numero = Convert.ToUInt16(outputstring, 16);
                                            convertido = numero;
                                            c += 3;
                                            doisbytes = false;
                                        }
                                    }
                                    else if (caractere == ' ')
                                    {
                                        convertido = 0x00;
                                        doisbytes = false;
                                    }
                                    else
                                    {
                                        convertido = tabela.hextoascii(caractere);
                                        ponteiro += 1;
                                        doisbytes = true;
                                    }

                                    if (doisbytes == true)
                                    {
                                        bw.Write(bigendian((uint)convertido));
                                        doisbytes = false;
                                    }
                                    else
                                    {
                                        bw.Write((uint)convertido);
                                        doisbytes = false;
                                    }
                                    ponteiro += 1;
                                }
                                //ponteiro += 1;//Se o jogo for texto 16bits coloque 2, se for 8bits coloque 1
                                proximoponteiro += 2;//Se o ponteiro for 32Bits coloque 4, se for 16Bits coloque 2
                            }
                        }

                        using (FileStream stream = File.Open(dump, FileMode.Open))
                        {
                            BinaryReader bro = new BinaryReader(stream);
                            BinaryWriter bw = new BinaryWriter(stream);

                            int magic = bro.ReadInt32(); // Read magic

                            int quantidadearquivos = bro.ReadInt32(); // Lê a a quantidade de arquivos

                            bw.BaseStream.Seek(0x0C, SeekOrigin.Begin);

                            int primeiroarquivo = bro.ReadInt32();

                            int offsetatual = primeiroarquivo;

                            // Inicia o array que vai guardar o nome de cada arquivo.
                            // Ele tem o +1 por começar em 0, e a variavel começar em 1, então precisa de um a mais
                            string[] nomearquivos = new string[quantidadearquivos + 1];

                            for (int i = 1; i <= quantidadearquivos; i++) // Faz o loop com cada arquivo
                            {
                                nomearquivos[i] = Path.Combine(pasta, arquivoaserescrito + "_" + i);
                            }

                            // Verifica se o arquivo já existe, apaga se existir ou cria se não existir
                            if (File.Exists(dump + "_novo"))
                            {
                                File.Delete(dump + "_novo");
                            }
                            else
                            {
                                // Cria o arquivo    
                                using (FileStream fs = File.Create(dump + "_novo"))
                                {
                                    BinaryReader brn = new BinaryReader(fs);
                                    BinaryWriter bwn = new BinaryWriter(fs);

                                    bro.BaseStream.Seek(0x0C, SeekOrigin.Begin);

                                    int tamanhocabecalho = bro.ReadInt32();

                                    bro.BaseStream.Seek(0, SeekOrigin.Begin);

                                    byte[] cabecalho = bro.ReadBytes(tamanhocabecalho); //Lê o cabeçalho do arquivo original

                                    bwn.Write(cabecalho); // Escreve o cabeçalho no arquivo novo

                                    for (int j = 1; j <= quantidadearquivos; j++) // Faz o loop com cada arquivo
                                    {

                                        FileInfo fi = new FileInfo(nomearquivos[j]);  //informa o tamanho do arquivo em bytes
                                        long tamanhoarquivonovo = (fi.Length); //tamanho arquivo novo recebe o valor do tamanho

                                        int tamanhonovo = Convert.ToInt32(tamanhoarquivonovo);

                                        byte[] conteudoarquivo = File.ReadAllBytes(nomearquivos[j]); //Lê todo o arquivo para a matriz conteudo do arquivo

                                        int resultado = Convert.ToInt32(tamanhoarquivonovo) / 16;

                                        int resultado2 = Convert.ToInt32(tamanhoarquivonovo) % 16;

                                        int blocos = resultado;

                                        if (resultado2 != 0)
                                        {
                                            blocos = resultado + 1;
                                        }

                                        int tamanhocompadding = blocos * 16;

                                        bwn.BaseStream.Seek(0x0C * j, SeekOrigin.Begin);

                                        bwn.Write(offsetatual);

                                        bwn.Write(tamanhonovo);

                                        bwn.BaseStream.Seek(offsetatual, SeekOrigin.Begin);

                                        //Escreve o arquivo
                                        bwn.Write(conteudoarquivo);

                                        offsetatual += tamanhocompadding;
                                    }
                                }
                            }
                        }
                        Directory.Delete(pasta, true); //Apaga a pasta temporaria e todos os arquivos
                    }
                    else
                    {
                        MessageBox.Show("Arquivo TXT não encontrado!", "AVISO!");

                    }
                }
                MessageBox.Show("Texto inserido e arquivo(s) remontado(s)!", "AVISO!");
            }
        }
    }
}