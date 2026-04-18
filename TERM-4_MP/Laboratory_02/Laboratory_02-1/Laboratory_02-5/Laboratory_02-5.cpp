#include "pch.h"
#include "Combi.h"

#define INF 2147483647 
#define SIZE 7
#define MAX_DISTANCE 300
#define MIN_DISTANCE 10
#define NO_ROAD 3

using namespace std;

void printMatrix(int** matrix, int size)
{
    cout << "Матрица расстояний между городами:" << endl;
    cout << "      ";
    for (int i = 0; i < size; i++) cout << setw(5) << i;
    cout << endl << "   " << string(size * 6, '-') << endl;

    for (int i = 0; i < size; i++)
    {
        cout << setw(3) << i << " | ";
        for (int j = 0; j < size; j++)
        {
            if (matrix[i][j] >= INF)
            {
                cout << setw(5) << "INF";
            }
            else
            {
                cout << setw(5) << matrix[i][j];
            }
        }
        cout << endl;
    }
    cout << endl;
}

void fillDistances(int** matrix, int size)
{
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if (i == j)
            {
                matrix[i][j] = INF;
            }
            else
            {
                matrix[i][j] = (rand() % (MAX_DISTANCE - MIN_DISTANCE)) + MIN_DISTANCE;
            }
        }
    }

    int blocked = 0;
    while (blocked < NO_ROAD) 
    {
        int r = rand() % size;
        int c = rand() % size;
        if (r != c && matrix[r][c] < INF)
        {
            matrix[r][c] = INF;
            blocked++;
        }
    }
}

int main() {
    setlocale(LC_CTYPE, "Russian");
    srand((unsigned int)time(NULL));

    int** distance_matrix = new int* [SIZE];
    for (int i = 0; i < SIZE; i++) distance_matrix[i] = new int[SIZE];

    cout << "Заполнение матрицы расстояний...\n";
    fillDistances(distance_matrix, SIZE);
    printMatrix(distance_matrix, SIZE);

    combi::Transposition transposition(SIZE);
    int min_weight = INF;
    int* best_route = new int[SIZE];
    long long counter = 0;

    cout << "-- Поиск минимального маршурта --" << endl;

    int index = 0;
    do {
        counter++;
        int route_weight = 0;
        bool has_blocked_path = false;

        for (int i = 0; i < SIZE; i++)
        {
            int u = transposition.sequence[i].element;
            int v = transposition.sequence[(i + 1) % SIZE].element;

            if (distance_matrix[u][v] >= INF)
            {
                has_blocked_path = true;
                break;
            }
            route_weight += distance_matrix[u][v];
        }

        if (!has_blocked_path && route_weight < min_weight)
        {
            min_weight = route_weight;
            for (int i = 0; i < SIZE; i++) best_route[i] = transposition.sequence[i].element;
        }

        index = transposition.getNextTransp();
    } while (index != -1);

    cout << "минимальный вес маршрута: " << min_weight << endl;
    cout << "найденный маршурт: ";
    for (int i = 0; i < SIZE; i++)
    {
        cout << best_route[i] << " -> ";
    }
    cout << best_route[0] << endl;
    cout << "Всего вариантов: " << counter << endl;

    for (int i = 0; i < SIZE; i++)
    {
        delete[] distance_matrix[i];
    }
    delete[] distance_matrix;
    delete[] best_route;

    return 0;
}