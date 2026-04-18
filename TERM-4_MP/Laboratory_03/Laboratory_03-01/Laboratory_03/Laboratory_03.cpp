#include "pch.h"
#include "Combi.h"

#define N 1
#define INF 17031

using namespace std;

#include <chrono>

class Timer {
private:
    using clock_t = std::chrono::steady_clock;
    using second_t = std::chrono::duration<double, std::ratio<1>>;

    std::chrono::time_point<clock_t> m_beg;
    bool m_running = false;
    double m_elapsed = 0.0;

public:
    Timer() : m_beg(clock_t::now()) {}

    void start() {
        m_beg = clock_t::now();
        m_running = true;
    }

    void stop() {
        if (m_running) {
            auto end = clock_t::now();
            m_elapsed = std::chrono::duration_cast<second_t>(end - m_beg).count();
            m_running = false;
        }
    }

    double getTime() const {
        if (m_running) {
            auto end = clock_t::now();
            return std::chrono::duration_cast<second_t>(end - m_beg).count();
        }
        return m_elapsed;
    }
};

bool isExcluded(const vector<short>& excluded_el, const int& index)
{
    for (int i = 0; i < excluded_el.size(); i++)
    {
        if (excluded_el[i] == index)
        {
            return true;
        }
    }
    return false;
}

void subMinRowElement(vector<vector<int>>& route_matrix, const vector<short>& excluded_rows, const vector<short>& excluded_cols, int row, int& min_path)
{
    if (isExcluded(excluded_rows, row))
    {
        return;
    }

    int min_el = INF;
    for (int i = 0; i < route_matrix[row].size(); i++)
    {
        if (isExcluded(excluded_cols, i))
        {
            continue;
        }

        if (route_matrix[row][i] < min_el)
        {
            min_el = route_matrix[row][i];
        }
    }

    if (min_el != INF && min_el > 0)
    {
        for (int i = 0; i < route_matrix[row].size(); i++)
        {
            if (isExcluded(excluded_cols, i))
            {
                continue;
            }

            if (route_matrix[row][i] != INF)
            {
                route_matrix[row][i] -= min_el;
            }
        }
        min_path += min_el;
    }
}

void subMinColElement(vector<vector<int>>& route_matrix, const vector<short>& excluded_rows, const vector<short>& excluded_cols, int col, int& min_path)
{
    if (isExcluded(excluded_cols, col))
    {
        return;
    }

    int min_el = INF;
    for (int i = 0; i < route_matrix.size(); i++)
    {
        if (isExcluded(excluded_rows, i))
        {
            continue;
        }

        if (route_matrix[i][col] < min_el)
        {
            min_el = route_matrix[i][col];
        }
    }

    if (min_el != INF && min_el > 0)
    {
        for (int i = 0; i < route_matrix.size(); i++)
        {
            if (isExcluded(excluded_rows, i))
            {
                continue;
            }

            if (route_matrix[i][col] != INF)
            {
                route_matrix[i][col] -= min_el;
            }
        }
        min_path += min_el;
    }
}

void printMatrix(const vector<vector<int>>& route_matrix, const vector<short>& excluded_rows, const vector<short>& excluded_cols)
{
    for (int i = 0; i < route_matrix.size(); i++)
    {
        if (isExcluded(excluded_rows, i))
        {
            continue;
        }

        for (int j = 0; j < route_matrix[i].size(); j++)
        {
            if (isExcluded(excluded_cols, j))
            {
                continue;
            }

            if (route_matrix[i][j] >= INF / 2)
            {
                cout << "INF";
            }
            else
            {
                cout << route_matrix[i][j];
            }
            cout << '\t';
        }
        cout << '\n';
    }
}

int calculatePenal(const vector<vector<int>>& route_matrix, const vector<short>& excluded_rows, const vector<short>& excluded_cols, int index_i, int index_j)
{
    int penalty = 0;
    int min_row = INF;
    int min_col = INF;

    for (int j = 0; j < route_matrix[index_i].size(); j++)
    {
        if (j == index_j || isExcluded(excluded_cols, j))
        {
            continue;
        }
        if (route_matrix[index_i][j] < min_row)
        {
            min_row = route_matrix[index_i][j];
        }
    }

    for (int i = 0; i < route_matrix.size(); i++)
    {
        if (i == index_i || isExcluded(excluded_rows, i))
        {
            continue;
        }
        if (route_matrix[i][index_j] < min_col)
        {
            min_col = route_matrix[i][index_j];
        }
    }

    penalty += min_row;
    penalty += min_col;

    return penalty;
}

void preventCycles(vector<vector<int>>& route_matrix, const vector<pair<int, int>>& path)
{
    if (path.size() >= route_matrix.size() - 1)
    {
        return;
    }

    for (int i = 0; i < path.size(); i++)
    {
        int start = path[i].first;
        int end = path[i].second;

        bool changed = true;
        while (changed)
        {
            changed = false;
            for (int j = 0; j < path.size(); j++)
            {
                if (path[j].first == end)
                {
                    end = path[j].second;
                    changed = true;
                    break;
                }
            }
        }

        changed = true;
        while (changed)
        {
            changed = false;
            for (int j = 0; j < path.size(); j++)
            {
                if (path[j].second == start)
                {
                    start = path[j].first;
                    changed = true;
                    break;
                }
            }
        }
        route_matrix[end][start] = INF;
    }
}

void findMaxPenaltyElement(vector<vector<int>>& route_matrix, vector<short>& excluded_rows, vector<short>& excluded_cols, vector<pair<int, int>>& path)
{
    int max_penalty = -1;
    int excluded_row = -1, excluded_col = -1;

    for (int i = 0; i < route_matrix.size(); i++)
    {
        if (isExcluded(excluded_rows, i))
        {
            continue;
        }

        for (int j = 0; j < route_matrix[i].size(); j++)
        {
            if (isExcluded(excluded_cols, j))
            {
                continue;
            }

            if (route_matrix[i][j] == 0)
            {
                int element_penalty = calculatePenal(route_matrix, excluded_rows, excluded_cols, i, j);
                if (element_penalty > max_penalty)
                {
                    max_penalty = element_penalty;
                    excluded_row = i;
                    excluded_col = j;
                }
            }
        }
    }

    if (excluded_row != -1 && excluded_col != -1)
    {
        excluded_rows.push_back(excluded_row);
        excluded_cols.push_back(excluded_col);
        path.push_back({ excluded_row, excluded_col });

        preventCycles(route_matrix, path);
    }
}

int main()
{
    setlocale(LC_CTYPE, "Russian");

    vector<vector<int>> route_matrix = {
        {INF, N, 19 + N, 3 + N, 22 - N},
        {1 + N, INF, 12 + N, 40 - N, 68 - N},
        {7 + N, 2 * N, INF, 72, 37 + N},
        {18 + N, 55 - N, 3 * N, INF, 2 * N},
        {85 - N, 32 + N, 45, 12 + N, INF}
    };

    int matrix_size = route_matrix.size();
    int min_path = 0;
    vector<short> excluded_rows, excluded_cols;
    vector<pair<int, int>> path;

    printMatrix(route_matrix, excluded_rows, excluded_cols);

    // Метод ветвей и границ
    Timer timer;
    timer.start();
    while (path.size() < matrix_size)
    {
        int current_path_size = path.size();

        for (int i = 0; i < matrix_size; i++)
        {
            subMinRowElement(route_matrix, excluded_rows, excluded_cols, i, min_path);
        }

        for (int i = 0; i < matrix_size; i++)
        {
            subMinColElement(route_matrix, excluded_rows, excluded_cols, i, min_path);
        }

        findMaxPenaltyElement(route_matrix, excluded_rows, excluded_cols, path);

        if (path.size() == current_path_size)
        {
            cout << "\nНет доступного ветвления\n";
            break;
        }

        cout << "\n-----------------------------------\n";
        cout << "Текущая нижняя граница: " << min_path << "\n";
        printMatrix(route_matrix, excluded_rows, excluded_cols);
    }

    timer.stop();
    
    cout << "\n-----------------------------------\n";
    cout << "Итоговый маршрут:\n";

    for (int i = 0; i < path.size(); i++)
    {
        cout << (path[i].first + 1) << " -> " << (path[i].second + 1) << "\n";
    }
    cout << "\nОбщая длина маршрута: " << min_path << "\n";
    cout << "Время, затраченое на поиск: " << timer.getTime();

    // Перестановки
    cout << "Решение задачи коммивояжера методов генерации всех сочетаний:\n";

    char initial_set[] = { 'A', 'B', 'C', 'D', 'E' };

    route_matrix = {
        {INF, N, 19 + N, 3 + N, 22 - N},
        {1 + N, INF, 12 + N, 40 - N, 68 - N},
        {7 + N, 2 * N, INF, 72, 37 + N},
        {18 + N, 55 - N, 3 * N, INF, 2 * N},
        {85 - N, 32 + N, 45, 12 + N, INF}
    };
    combi::Transposition transposition(sizeof(initial_set));
    short counter = 0;
    min_path = INF;
    int index = -1;
    timer.start();
    do
    {
        short path = 0;
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
        cout << '}';
        for (int i = 0; i < transposition.sequence_size - 1; i++)
        {
            if (route_matrix[transposition.sequence[i].element][transposition.sequence[i + 1].element] == INF)
            {
                path = INF;
                break;
            }
            path += route_matrix[transposition.sequence[i].element][transposition.sequence[i + 1].element];
        }
        if (path != INF)
        {
            path += route_matrix[transposition.sequence[transposition.sequence_size - 1].element][transposition.sequence[0].element];
        }
        if (min_path > path)
        {
            min_path = path;
        }
        cout << "Минимальный путь сейчас: ";
        if (min_path != INF)
        {
            cout << min_path;
        }
        else
        {
            cout << "INF";
        }
        cout << endl;
        index = transposition.getNextTransp();
    } while (index != -1);
    timer.stop();
    cout << "Всего перестановок: " << counter << endl;
    cout << "Минимальная длина маршрута: " << min_path;
    cout << "Время, затраченое на поиск: " << timer.getTime();
    return 0;
}