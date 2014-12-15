all: matrix

matrix: main.o matrices.o
	g++ -c -Wall main.cpp  -o matrices

main.o: main.cpp
	g++ -c -Wall main.cpp

matrices.o: matrices.cpp
	g++ -c -Wall matrices.cpp

clean:
	rm -rf *.o matrix