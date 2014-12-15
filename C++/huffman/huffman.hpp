#pragma once
#include <vector>
#include <map>
#include <stdint.h>

const size_t Size = 256;
const size_t tableSize = 1024;
const uint32_t byteSize = 8;
 
enum Mode{
	ENCODING,
	DECODING
};
enum ErrorType{
	NONE,
	INPUT_ERR,
	INPUT_FILE_ERR,
	OUTPUT_FILE_ERR,
	WRONG_ARGUMENTS_AMOUNT
};

struct TreeNode{
    unsigned char value;
    uint32_t key;
    TreeNode* left;
    TreeNode* right;
}; 

struct NodeComparator{
	bool operator() ( TreeNode * const a, TreeNode * const b) const 
		{return (a->key == b->key ? a->value < b->value : a->key < b->key);}
};

struct Tree{
    TreeNode* root;
};

struct Huffman{
	Huffman();
	ErrorType error;
	Mode archivationMode;
	void encode(std::string const &, std::string const &);
	void decode(std::string const &, std::string const &);
	private :
		uint32_t FileLength;
		Tree* HuffmanTree;
		std::vector<uint32_t> FrequencyTable;
		std::vector<std::string> CodeList;
		void clear();
		void getBits(TreeNode*, std::string const &);
		void buildCodeTree();
		void writeStringToFile(std::string const &, std::string const &);
		std::string readStringFromFile(std::string const &);
		unsigned char findBit(TreeNode*, std::string const &, uint32_t*);

};

TreeNode* init_node(uint32_t, char,TreeNode*, TreeNode*);
Tree* init_tree();
void deleteNode(TreeNode*);
void deleteTree(Tree*);

 