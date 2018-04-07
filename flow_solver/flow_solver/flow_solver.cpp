// flow_solver.cpp : Defines the entry point for the console application.
//
#include <iostream>

#include "stdafx.h"
using namespace std;

class Point {
public:
	int x;
	int y;
	int color;
	Point(int x, int y, int color);
	~Point();
};


Point::Point(int x, int y, int color) {
	x = x;
	y = y;
	color = color;
};

Point::~Point(void) {
	x = 0;
	y = 0;
	color = 0;
};



int main()
{
	return 0;
}

