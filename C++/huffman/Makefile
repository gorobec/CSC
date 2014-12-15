all: huffman

huffman: main.o huffman.o
	g++ -w -std=c++11 main.o huffman.o -o huffman
	
main.o: main.cpp
	g++ -w -std=c++11 -c main.cpp

huffman.o: huffman.cpp
	g++ -w -std=c++11 -c huffman.cpp
	
clean:
	rm -rf *.o huffman