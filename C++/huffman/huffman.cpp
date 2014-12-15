#include <set>
#include <string>
#include <fstream> 
#include <iostream>
#include "huffman.hpp"

using namespace std;

Huffman::Huffman()
{
	archivationMode = ENCODING;
	FrequencyTable.resize(Size);
	CodeList.resize(Size);
	FileLength = 0;
	error = NONE;
}
TreeNode* init_node(uint32_t key, char val, TreeNode* left, TreeNode* right)
{
    TreeNode* res = new TreeNode();
    res->key = key;
    res->value = val;
    res->left = left;
    res->right = right;
    return res;
}
 
Tree* init_tree(TreeNode* root)
{
    Tree* res = new Tree();
    res->root = root;
    return res;
}

void deleteNode(TreeNode* node)
{
	if (node->left != NULL)
		{deleteNode(node->left);}
	if (node->right != NULL)
		{deleteNode(node->right);}
	delete node;
}

void deleteTree(Tree* t)
{
	deleteNode(t->root);
	delete t;
}

void Huffman::clear()
{
	deleteTree(HuffmanTree);
	CodeList.clear();
	FrequencyTable.clear();
	FrequencyTable.resize(Size);
	CodeList.resize(Size);
	error = NONE;
}
 
string Huffman::readStringFromFile(string const & INPUT)
{
	ifstream input (INPUT.c_str(), std::ofstream::binary);
	if (input) 
	{
		input.seekg (0, input.end);
		size_t length = input.tellg();
		input.seekg (0, input.beg);

		char * buffer = new char [length];
		input.read (buffer, length);

		if (this->archivationMode == ENCODING)
		{
			for (size_t i = 0; i != length; ++i){
				FrequencyTable[(unsigned char)buffer[i]]++;}
		}
		else
		{
			if (length == 0)
			{
				delete[] buffer; 
				return "";
			} 
			for (size_t i = 0; i != tableSize; i += 4)
			{
				uint32_t quantity = ((unsigned char)buffer[i] << (byteSize * 3)) | ((unsigned char)buffer[i + 1] << (byteSize * 2)) |
					((unsigned char)buffer[i + 2] << byteSize) | (unsigned char)buffer[i + 3];
				FileLength += quantity;
				FrequencyTable[i/4] = quantity;
			}
		}
		string s (buffer, length);
		delete[] buffer;
		return s;
	}
	else 
	{
		error = INPUT_FILE_ERR;
		return "Wrong input file";
	}
}

void Huffman::writeStringToFile(string const & OUTPUT, string const & result)
{
	std::ofstream output (OUTPUT, std::ofstream::binary);
	if (output)
	{
		output.write (result.c_str(), result.length());
		output.close();
	}
	else 
	{error = OUTPUT_FILE_ERR;}
}

void Huffman::getBits(TreeNode* node, string const & s)
{
	if (node->left == NULL && node->left == NULL)
		{this->CodeList[node->value] = s;}

	if (node->left != NULL)
		{getBits(node->left, s + "0");}

	if (node->right != NULL)
		{getBits(node->right, s + "1");}
}

void Huffman::buildCodeTree()
{
    multiset<TreeNode*, NodeComparator> priority;

    for (size_t id = 0; id != Size; id++)
    {
		if (FrequencyTable[id])
        {
            TreeNode* node = init_node(FrequencyTable[id], (unsigned char)id, NULL, NULL);
            priority.insert(node);
        }
    }
    while (priority.size() != 1)
    {
		TreeNode* left = *priority.begin();
        priority.erase(priority.begin());
 
        TreeNode* right = *priority.begin();
        priority.erase(priority.begin());
 
		TreeNode* new_node = init_node(left->key + right->key, '$', left, right);
        priority.insert(new_node);
    }

	HuffmanTree = init_tree(*priority.begin());

	if (HuffmanTree->root->left == NULL && HuffmanTree->root->right == NULL)
	{
		TreeNode* root = init_node(HuffmanTree->root->key, '$', HuffmanTree->root, NULL);
		HuffmanTree->root = root;
	}
	getBits(HuffmanTree->root, "");
}
 
void Huffman::encode(string const & INPUT, string const & OUTPUT)
{
	string result;
	string bitString;
	string toEncode = readStringFromFile(INPUT);
	if (error)
		{return;}
	if (toEncode.length() == 0)
	{
		cout << 0 << endl << 0 << endl << 0 << endl;
		writeStringToFile(OUTPUT, "");
		return;
	}
	buildCodeTree();
	cout << toEncode.length() << endl;

	for (size_t i = 0; i != toEncode.length(); ++i)
		{bitString += CodeList[(unsigned char)toEncode[i]];}

	unsigned char code = 0;
	for (size_t i = 0; i != Size; ++i)
	{
		result += (FrequencyTable[i] & 0xFF000000) >> (byteSize * 3);
		result += (FrequencyTable[i] & 0x00FF0000) >> (byteSize * 2);
		result += (FrequencyTable[i] & 0x0000FF00) >> byteSize;
		result += FrequencyTable[i] & 0x000000FF;
	}
	size_t iter = 0;
	while (iter != bitString.length())
	{
		code = code | (((bitString[iter] - '0') & 1) << (iter % 8));
		++iter;
		if (iter % 8 == 0 && iter != 0)
		{
			result += code;
			code = 0;
		}
	}
	if (iter % 8)
		{result += code;}
	
	cout << result.length() - tableSize << endl << tableSize << endl;
	writeStringToFile(OUTPUT, result);
	clear();
}

void Huffman::decode(string const & INPUT, string const & OUTPUT)
{
	string toDecode = readStringFromFile(INPUT);
	if (error)
		{return;}
	if (toDecode.length() == 0)
	{
		cout << 0 << endl << 0 << endl << 0 << endl;
		writeStringToFile(OUTPUT, "");
		return;
	}
	toDecode = toDecode.substr(tableSize, toDecode.length() - tableSize);
	cout << toDecode.length() << endl;

	buildCodeTree();
	string code;
	                    
	for (size_t it = 0; it != toDecode.length(); ++it)
	{
		unsigned char bit = toDecode[it];
		for (size_t j = 0; j != 8; ++j)
		{
			code += (unsigned char)((bit & 1) + '0');
			bit >>= 1;
		}
	}
	uint32_t i = 0;
	string result;

 	while (i < code.length())
	{
		TreeNode* node = HuffmanTree->root;
		while (true)
		{
			if (node->left == NULL && node->right == NULL)
			{
				result += node->value;
				break;
			}

			if (code[i++] == '0')
				{node = node->left;}
			else
				{node = node->right;}
		}
		if (result.length() == FileLength)
			{break;}
	}
	
	cout << result.length() << endl << tableSize << endl;
	writeStringToFile(OUTPUT, result);
	clear();
}