#include "pch.h"
#include "Combi.h"

using namespace std;

int main()
{
    setlocale(LC_CTYPE, "Russian");

    char initial_set[] = { 'A', 'B', 'C', 'D', 'F'};

    short n = sizeof(initial_set) / sizeof(initial_set[0]);
    short m = 4;

    combi::Accomodation acc(n, m);

    cout << "-- Генератор размещений (из " << n << " по " << m << ") --\n\n";

    int counter = 0;

    if (acc.getfirst() != -1)
    {
        do
        {
            counter++;
            cout << setw(3) << counter << ": { ";
            for (int i = 0; i < m; i++)
            {
                cout << initial_set[acc.sset[i]];
                if (i < m - 1)
                {
                    cout << ", ";
                }
            }
            cout << " }" << endl;

        } while (acc.getnext() != -1);
    }
    else
    {
        cout << "Ошибка: m > n или некорректные данные." << endl;
    }

    cout << "\nВсего найдено: " << counter << endl;
    cout << "Расчет по формуле: " << acc.count() << endl;

    return 0;
}