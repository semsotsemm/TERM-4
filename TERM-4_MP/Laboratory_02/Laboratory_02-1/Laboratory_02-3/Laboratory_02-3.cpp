#include "pch.h"
#include "Combi.h"

using namespace std;

int main()
{
	setlocale(LC_CTYPE, "Russian");

	char initial_set[] = { 'A', 'B', 'C', 'D'};
	combi::Transposition transposition(sizeof(initial_set));

	cout << "-- Генератор перестановок --\n\n";
	short counter = 0;
	int index = -1;
	do
	{
		counter++;
		cout << '{';
		for (int i = 0; i < transposition.sequence_size; i++)
		{
			cout << transposition.sequence[i].element;
			if (i + 1 < transposition.sequence_size)
			{
				cout << ", ";
			}
		}
		cout << '}' << endl;
		index = transposition.getNextTransp();
	} while (index != -1);
	cout << "Всего перестановок: " << counter;
}