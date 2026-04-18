#include "pch.h"
#include "Combi.h"

#define INF 2147483647 
#define MIN_SIZE 6    
#define MAX_SIZE 12   
#define MAX_DISTANCE 300
#define MIN_DISTANCE 10
#define NO_ROAD 3

using namespace std;
using namespace std::chrono;

void printMatrix(int** matrix, int size)
{
    cout << "Матрица расстояний (" << size << " городов):" << endl;
    cout << "      ";
    for (int i = 0; i < size; i++) cout << setw(5) << i;
    {
        cout << endl << "   " << string(size * 6, '-') << endl;
    }

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

    for (int current_size = MIN_SIZE; current_size <= MAX_SIZE; current_size++)
    {

        cout << "\n----------------- Тест для " << current_size << " городов -----------------\n";

        int** distance_matrix = new int* [current_size];
        for (int i = 0; i < current_size; i++)
        {
            distance_matrix[i] = new int[current_size];
        }

        fillDistances(distance_matrix, current_size);
        printMatrix(distance_matrix, current_size);

        combi::Transposition transposition(current_size);
        int min_weight = INF;
        int* best_route = new int[current_size];
        long long counter = 0;

        auto start_time = steady_clock::now();

        int index = 0;
        do {
            counter++;
            int route_weight = 0;
            bool has_blocked_path = false;

            for (int i = 0; i < current_size; i++) 
            {
                int u = transposition.sequence[i].element;
                int v = transposition.sequence[(i + 1) % current_size].element;

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
                for (int i = 0; i < current_size; i++)
                {
                    best_route[i] = transposition.sequence[i].element;
                }
            }

            index = transposition.getNextTransp();
        } while (index != -1);

        auto end_time = steady_clock::now();

        duration<double> diff = end_time - start_time;

        if (min_weight >= INF)
        {
            cout << "Маршрут не найден (везде INF)." << endl;
        }
        else
        {
            cout << "Минимальный вес: " << min_weight << endl;
            cout << "Лучший маршрут: ";
            for (int i = 0; i < current_size; i++)
            {
                cout << best_route[i] << " -> ";
            }
            cout << best_route[0] << endl;
        }
        cout << "Всего перестановок: " << counter << endl;
        cout << "Время вычислений: " << fixed << setprecision(3) << diff.count() << " сек." << endl;

        for (int i = 0; i < current_size; i++)
        {
            delete[] distance_matrix[i];
        }
        delete[] distance_matrix;
        delete[] best_route;
    }

    cout << "\nТестирование завершено." << endl;
    return 0;
}