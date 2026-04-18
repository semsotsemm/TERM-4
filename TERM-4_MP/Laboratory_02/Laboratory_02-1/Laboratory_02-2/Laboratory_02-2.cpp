#include "pch.h"
#include "Combi.h"

using namespace std;

int main()
{
	setlocale(LC_CTYPE, "Russian");

	// [0 2 3 | 5 0]
	char initial_set[] = { 'A', 'B', 'C', 'D', 'F' };
	short m = 3;
	combi::Combination combination(sizeof(initial_set), m);

	cout << "-- Генератор сочетаний --\n\n";
	cout << "Сочетания из " << sizeof(initial_set) << " по " << m << endl;

	for (int i = 0; i < combination.count_combinations(); i++)
	{
		cout << '{';

		bool flag = false;
		for (int j = 0; j < m; j++)
		{
			cout << initial_set[combination.sequence[j]];
			if (j != m - 1)
			{
				cout << ',';
			}
			cout << ' ';
		}
		cout << '}' << endl;
		combination.getNextCombination();
	}
	cout << "\nКоличество подмножеств: " << combination.count_combinations() << endl;
}