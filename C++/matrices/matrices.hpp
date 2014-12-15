#pragma once

enum ErrorType{
	NONE,
	SIZE_ERR,
	INPUT_ERR,
	NO_INPUT_ERR,
	WRONG_ARGUMENTS_AMOUNT
};

struct Matrix{
	size_t height;
	size_t width;
	double** value;
	ErrorType error;

	Matrix(char const* file_name);
	void clear();
	void print();
	void add(Matrix const &);
	void mult(Matrix const &);
};
