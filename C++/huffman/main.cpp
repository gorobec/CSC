#include <iostream>
#include <string.h>
#include "huffman.hpp"

using namespace std;
 
int main(int argc, char *argv[])
{
	if (argc < 6)
	{
		cerr << "Wrong amount of argunents" << endl;
		return WRONG_ARGUMENTS_AMOUNT;
	}
	Huffman huffman = Huffman();
	string INPUT, OUTPUT;

	for (int i = 1; i != argc; ++i)
	{
		if ((!strcmp(argv[i], "-c")) && (!strcmp(argv[i + 1],"-f")))
		{
			i += 2;
			huffman.archivationMode = ENCODING;
			INPUT = argv[i];
		}
		else if ((!strcmp(argv[i], "-u")) && (!strcmp(argv[i + 1],"-f")))
		{
			i += 2;
			huffman.archivationMode = DECODING;
			INPUT = argv[i];
		}
		else if (!strcmp(argv[i], "-o"))
		{
			++i;
			OUTPUT = argv[i];
			if (huffman.archivationMode == ENCODING)
				{huffman.encode(INPUT, OUTPUT);}
			else 
				{huffman.decode(INPUT, OUTPUT);}

			if (huffman.error)
			{
				if(huffman.error == INPUT_FILE_ERR)
					{cerr << "Wrong input file";}
				else if (huffman.error == OUTPUT_FILE_ERR)
					{cerr << "Wrong output file";}
				return huffman.error;
			}
		}
		else
		{
			cerr << "Wrong input" << endl;
			return INPUT_ERR;
		}
	}
    return 0;
}