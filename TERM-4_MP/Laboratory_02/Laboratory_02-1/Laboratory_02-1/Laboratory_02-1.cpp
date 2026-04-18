#include "pch.h"
#include "Combi.h"

using namespace std;

int main()
{
	setlocale(LC_CTYPE, "Russian");

	char initial_set[] = {'A', 'B', 'C', 'D', 'F'};

	combi::Subset subset(initial_set, sizeof(initial_set) / sizeof(char));
	cout << "-- Генератор подмножеств --\n\n";
	cout << "Комбинации:\n";

	for (int i = 0; i < subset.quantity_subset; i++)
	{
		subset.getNextSubset();
		cout << '{';

		bool flag = false;
		for (int j = 0; j < subset.size_set; j++)
		{
			if (*(subset.included_elements + j) != 0)
			{
				if (flag)
				{
					cout << ", ";
				}
				flag = true;
				cout << initial_set[*(subset.included_elements + j) - 1];
			}
		}
		cout << '}' << endl;
		subset.clearSubset();
		subset.mask += 1;
	}
	cout << "\nКоличество подмножеств: " << subset.quantity_subset << endl;
}