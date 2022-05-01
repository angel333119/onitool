namespace Onitool
{
    class tabela
    {
        public static string Converterascii(int comparador)
        {
            string convertido;
            //return convertido; tá la no final.

            if (comparador >= 0x10 && comparador <= 0x19) //Começa a conversão dos caracteres
            {
                convertido = ((char)('0' + (comparador - 0x10))).ToString(); //Convertendo dentro do intervalo de 0 e 9.
            }
            else if (comparador >= 0x21 && comparador <= 0x3A)
            {
                convertido = ((char)('A' + (comparador - 0x21))).ToString(); //Convertendo dentro do intervalo de A e Z.
            }
            else if (comparador >= 0x41 && comparador <= 0x5A)
            {
                convertido = ((char)('a' + (comparador - 0x41))).ToString(); //Convertendo dentro do intervalo de a e z.
            }
            else if (comparador == 0x01)
            {
                convertido = ("!");
            }
            else if (comparador == 0x02)
            {
                convertido = '"'.ToString();
            }
            else if (comparador == 0x03)
            {
                convertido = ("#");
            }
            else if (comparador == 0x04)
            {
                convertido = ("$");
            }
            else if (comparador == 0x05)
            {
                convertido = ("%");
            }
            else if (comparador == 0x06)
            {
                convertido = ("&");
            }
            else if (comparador == 0x07)
            {
                convertido = ("'");
            }
            else if (comparador == 0x08)
            {
                convertido = ("(");
            }
            else if (comparador == 0x09)
            {
                convertido = (")");
            }
            else if (comparador == 0x0A)
            {
                convertido = ("*");
            }
            else if (comparador == 0x0B)
            {
                convertido = ("+");
            }
            else if (comparador == 0x0C)
            {
                convertido = (",");
            }
            else if (comparador == 0x0D)
            {
                convertido = ("-");
            }
            else if (comparador == 0x0E)
            {
                convertido = (".");
            }
            else if (comparador == 0x0F)
            {
                convertido = ("/");
            }
            else if (comparador == 0x1A)
            {
                convertido = (":");
            }
            else if (comparador == 0x1B)
            {
                convertido = (";");
            }
            else if (comparador == 0x1C)
            {
                convertido = ("<");
            }
            else if (comparador == 0x1D)
            {
                convertido = ("=");
            }
            else if (comparador == 0x1E)
            {
                convertido = (">");
            }
            else if (comparador == 0x1F)
            {
                convertido = ("?");
            }
            else if (comparador == 0x68)
            {
                convertido = ("@");
            }
            else if (comparador == 0x7E)
            {
                convertido = ("®");
            }
            else if (comparador == 0x74)
            {
                convertido = ("…");
            }
            else if (comparador == 0x7D)
            {
                convertido = ("►");
            }
            else if (comparador == 0x80)
            {
                convertido = ("Œ");
            }
            else if (comparador == 0x81)
            {
                convertido = ("œ");
            }            
            else if (comparador == 0x89)
            {
                convertido = ("À");
            }
            else if (comparador == 0xA1)
            {
                convertido = ("Á");
            }
            else if (comparador == 0x8B)
            {
                convertido = ("Â");
            }
            else if (comparador == 0x82)
            {
                convertido = ("Ã");
            }
            else if (comparador == 0x9B)
            {
                convertido = ("Ç");
            }
            else if (comparador == 0x8D)
            {
                convertido = ("È");
            }
            else if (comparador == 0x8F)
            {
                convertido = ("É");
            }
            else if (comparador == 0x91)
            {
                convertido = ("Ê");
            }
            else if (comparador == 0x9F)
            {
                convertido = ("Ë");
            }
            else if (comparador == 0xA9)
            {
                convertido = ("Ì");
            }
            else if (comparador == 0xA3)
            {
                convertido = ("Í");
            }
            else if (comparador == 0x95)
            {
                convertido = ("Î");
            }
            else if (comparador == 0x93)
            {
                convertido = ("Ï");
            }
            else if (comparador == 0x9D)
            {
                convertido = ("Ñ");
            }
            else if (comparador == 0xAB)
            {
                convertido = ("Ò");
            }
            else if (comparador == 0xA5)
            {
                convertido = ("Ó");
            }
            else if (comparador == 0x97)
            {
                convertido = ("Ô");
            }
            else if (comparador == 0x84)
            {
                convertido = ("Õ");
            }
            else if (comparador == 0x99)
            {
                convertido = ("Ù");
            }
            else if (comparador == 0xA7)
            {
                convertido = ("Ú");
            }
            else if (comparador == 0x86)
            {
                convertido = ("Ü");
            }
            else if (comparador == 0x8A)
            {
                convertido = ("à");
            }
            else if (comparador == 0xA2)
            {
                convertido = ("á");
            }
            else if (comparador == 0x8C)
            {
                convertido = ("â");
            }
            else if (comparador == 0x83)
            {
                convertido = ("ã");
            }
            else if (comparador == 0x9C)
            {
                convertido = ("ç");
            }
            else if (comparador == 0x8E)
            {
                convertido = ("è");
            }
            else if (comparador == 0x90)
            {
                convertido = ("é");
            }
            else if (comparador == 0x92)
            {
                convertido = ("ê");
            }
            else if (comparador == 0xA0)
            {
                convertido = ("ë");
            }
            else if (comparador == 0xAA)
            {
                convertido = ("ì");
            }
            else if (comparador == 0xA4)
            {
                convertido = ("í");
            }
            else if (comparador == 0x96)
            {
                convertido = ("î");
            }
            else if (comparador == 0x94)
            {
                convertido = ("ï");
            }
            else if (comparador == 0x9E)
            {
                convertido = ("ñ");
            }
            else if (comparador == 0xAC)
            {
                convertido = ("ò");
            }
            else if (comparador == 0xA6)
            {
                convertido = ("ó");
            }
            else if (comparador == 0x98)
            {
                convertido = ("ô");
            }
            else if (comparador == 0x85)
            {
                convertido = ("õ");
            }
            else if (comparador == 0x9A)
            {
                convertido = ("ù");
            }
            else if (comparador == 0xA8)
            {
                convertido = ("ú");
            }
            else if (comparador == 0x87)
            {
                convertido = ("ü");
            }            
            else
            {
                //Os valores que não estiverem na tabela, serão colocados em hex entre <>
                convertido = ("<" + comparador.ToString("X2") + ">");
                //convertido = comparador.ToString("X4");       mostra o valor em hex
                //comparador.ToString("<" + comparador + ">");  colocar o valor entre <>
            }
            return convertido;
        }


        public static int hextoascii(char caractere)
        {
            int convertido = 0;

            if (caractere >= '0' && caractere <= '9')
            {
                //Convertendo dentro do intervalo de a e z.
                convertido = (0xFA10 + (caractere - '0'));
            }
            else if (caractere >= 'A' && caractere <= 'Z')
            {
                //Convertendo dentro do intervalo de A e Z.
                convertido = (0xFA21 + (caractere - 'A'));
            }
            else if (caractere >= 'a' && caractere <= 'z')
            {
                //Convertendo dentro do intervalo de a e z.
                convertido = (0xFA41 + (caractere - 'a'));
            }
            else if (caractere == '!')
            {
                convertido = 0xFA01;
            }
            else if (caractere == '"')
            {
                convertido = 0xFA02;
            }
            else if (caractere == '#')
            {
                convertido = 0xFA03;
            }
            else if (caractere == '$')
            {
                convertido = 0xFA04;
            }
            else if (caractere == '%')
            {
                convertido = 0xFA05;
            }
            else if (caractere == '&')
            {
                convertido = 0xFA06;
            }
            else if (caractere == '\'')
            {
                convertido = 0xFA07;
            }
            else if (caractere == '(')
            {
                convertido = 0xFA08;
            }
            else if (caractere == ')')
            {
                convertido = 0xFA09;
            }
            else if (caractere == '+')
            {
                convertido = 0xFA0B;
            }
            else if (caractere == ',')
            {
                convertido = 0xFA0C;
            }
            else if (caractere == '-')
            {
                convertido = 0xFA0D;
            }
            else if (caractere == '.')
            {
                convertido = 0xFA0E;
            }
            else if (caractere == '/')
            {
                convertido = 0xFA0F;
            }
            else if (caractere == ':')
            {
                convertido = 0xFA1A;
            }
            else if (caractere == ';')
            {
                convertido = 0xFA1C;
            }
            else if (caractere == '?')
            {
                convertido = 0xFA1F;
            }
            else if (caractere == '@')
            {
                convertido = 0xFA68;
            }
            else if (caractere == '®')
            {
                convertido = 0xFA7E;
            }
            else if (caractere == '…')
            {
                convertido = 0xFA74;
            }
            else if (caractere == '►')
            {
                convertido = 0xFA7D;
            }
            else if (caractere == 'Œ')
            {
                convertido = 0xFA80;
            }
            else if (caractere == 'œ')
            {
                convertido = 0xFA81;
            }
            else if (caractere == 'À')
            {
                convertido = 0xFA89;
            }
            else if (caractere == 'Á')
            {
                convertido = 0xFAA1;
            }
            else if (caractere == 'Â')
            {
                convertido = 0xFA8B;
            }
            else if (caractere == 'Ã')
            {
                convertido = 0xFA82;
            }
            else if (caractere == 'Ç')
            {
                convertido = 0xFA9B;
            }
            else if (caractere == 'É')
            {
                convertido = 0xFA8F;
            }
            else if (caractere == 'Ê')
            {
                convertido = 0xFA91;
            }
            else if (caractere == 'Í')
            {
                convertido = 0xFAA3;
            }
            else if (caractere == 'Ó')
            {
                convertido = 0xFAA5;
            }
            else if (caractere == 'Ô')
            {
                convertido = 0xFA97;
            }
            else if (caractere == 'Õ')
            {
                convertido = 0xFA84;
            }
            else if (caractere == 'Ú')
            {
                convertido = 0xFAA7;
            }
            else if (caractere == 'à')
            {
                convertido = 0xFA8A;
            }
            else if (caractere == 'á')
            {
                convertido = 0xFAA2;
            }
            else if (caractere == 'â')
            {
                convertido = 0xFA8C;
            }
            else if (caractere == 'ã')
            {
                convertido = 0xFA83;
            }
            else if (caractere == 'ç')
            {
                convertido = 0xFA9C;
            }
            else if (caractere == 'é')
            {
                convertido = 0xFA90;
            }
            else if (caractere == 'ê')
            {
                convertido = 0xFA92;
            }
            else if (caractere == 'í')
            {
                convertido = 0xFAA4;
            }
            else if (caractere == 'ó')
            {
                convertido = 0xFAA6;
            }
            else if (caractere == 'ô')
            {
                convertido = 0xFA98;
            }
            else if (caractere == 'õ')
            {
                convertido = 0xFA85;
            }
            else if (caractere == 'ú')
            {
                convertido = 0xFAA8;
            }
            return convertido;
        }
    }
}

