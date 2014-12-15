#include <iostream>
#include <fstream>
#include "matrices.hpp"

using namespace std;

int main(int argc, char ** argv)
{
	if(argc == 0)
	{
		cerr << "No arguments found";
		return NO_INPUT_ERR;
	}
	else if(argc % 2)
	{
		cerr << "Wrong amount of arguments";
		return WRONG_ARGUMENTS_AMOUNT;
	}
	Matrix result(argv[1]);

	for (size_t i = 2; i != argc; ++i)
	{
		if (argv[i] == "--add")
		{
			Matrix temporary(argv[++i]);
			result.add(temporary);
			if (result.error == SIZE_ERR)
			{
				cerr << "Invalid matrix size" << endl;
				return result.error;
			}
		}
		else if (argv[i] == "--mult")
		{
			Matrix temporary(argv[++i]);
			result.mult(temporary);
			if (result.error == SIZE_ERR)
			{
				cerr << "Invalid matrix size" << endl;
				return result.error;
			}
		}
		else
		{
			cerr << "Wrong input" << endl;
			return INPUT_ERR;
		}
	}
	result.print();
	result.clear();
	return 0;
}