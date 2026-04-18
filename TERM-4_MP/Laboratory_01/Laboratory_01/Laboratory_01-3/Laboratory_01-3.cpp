#include <iostream>
#include <ctime>   

using namespace std;

// 0 1 1 2 3 5 8
int getFibonacciNumber(int number)
{
	if (number == 0 || number == 1)
	{
		return number;
	}

	return getFibonacciNumber(number - 1) + getFibonacciNumber(number - 2);
}

int main()
{
	setlocale(LC_CTYPE, "Russian");

	int result;
	clock_t time_start, time_end;

	for (int i = 1; i < 35; i++)
	{
		time_start = clock();
		result = getFibonacciNumber(i);
		time_end = clock();
		cout << i << " число Фибаначчи: " << result << " вычислено за: " << time_end - time_start << endl;
	}

	return 0;
}