#include <iostream>
#include <fstream>
#include "matrices.hpp"

using namespace std;

Matrix::Matrix(char const* file_name)
{
	ifstream input(file_name);
	input >> height >> width;
	error = NONE;
	value = new double* [height];
	value[0] = new double [width * height];

	for (size_t i = 1; i != height; ++i)
	{
		value[i] = value[i - 1] + width;
	}

	for (size_t i = 0; i != height; ++i)
		for (size_t j = 0; j != width; ++j)
		{
			input >> value[i][j];
		}
}

void Matrix::clear()
{
	delete[] value[0];
	delete[] value;
}

void Matrix::print()
{
	for (size_t i = 0; i != height; ++i)
	{
		for (size_t j = 0; j != width; ++j)
		{
			cout << value[i][j] << ' ';
		}
		cout << endl;
	}
}
void Matrix::add(Matrix const &second_matrix)
{
	if (height != second_matrix.height || width != second_matrix.width)
	{
		error = SIZE_ERR;
		return;
	}
	for (size_t i = 0; i != height; ++i)
		for (size_t j = 0; j != width; ++j)
		{
			value[i][j] += second_matrix.value[i][j];
		}
}

void Matrix::mult(Matrix const &second_matrix)
{
	if (width != second_matrix.height)
	{
		error = SIZE_ERR;
		return;
	}
	double** new_value = new double* [height];
	new_value[0] = new double [second_matrix.width * height];

	for (size_t i = 1; i != height; ++i)
	{
		new_value[i] = new_value[i - 1] + second_matrix.width;
	}
	
	for (size_t i = 0; i != height; ++i)
		for (size_t j = 0; j != second_matrix.width; ++j)
		{
			new_value[i][j] = 0;
			for (size_t k = 0; k != width; ++k)
			{
				new_value[i][j] += value[i][k] * second_matrix.value[k][j];
			}
		}
	width = second_matrix.width;
	value = new_value;
}
