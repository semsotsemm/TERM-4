#include <iostream>
#include <vector>
#include <iomanip>
#include <algorithm>
#include <string>

#define N 1

using namespace std;

const int INF = 1e9;

struct Target {
    int r, c, val;
    int penalty;
};

void printTable(int iter, const vector<vector<int>>& cost, const vector<vector<int>>& alloc,
    const vector<int>& s, const vector<int>& d,
    const vector<int>& rp, const vector<int>& cp,
    const vector<bool>& rd, const vector<bool>& cd) {

    cout << "\n--- ИТЕРАЦИЯ №" << iter << " ---" << endl;
    cout << setfill(' ') << setw(10) << "Пст\\Потр";
    for (int j = 0; j < 6; j++) cout << setw(10) << "B" + to_string(j + 1);
    cout << setw(12) << "Запасы" << setw(10) << "Штраф" << endl;
    cout << string(95, '-') << endl;

    for (int i = 0; i < 6; i++) {
        cout << setw(10) << (i == 5 ? "A6 (Ф)" : "A" + to_string(i + 1));
        for (int j = 0; j < 6; j++) {
            if (rd[i] || cd[j]) {
                if (alloc[i][j] > 0) cout << setw(6) << cost[i][j] << "(" << left << setw(3) << alloc[i][j] << ")" << right;
                else cout << setw(10) << "-";
            }
            else {
                if (alloc[i][j] > 0) cout << setw(6) << cost[i][j] << "(" << left << setw(3) << alloc[i][j] << ")" << right;
                else cout << setw(10) << cost[i][j];
            }
        }
        cout << setw(12) << s[i] << setw(10) << (rp[i] == -1 ? "X" : to_string(rp[i])) << endl;
    }

    cout << string(95, '-') << endl;
    cout << setw(10) << "Потр.";
    for (int j = 0; j < 6; j++) cout << setw(10) << d[j];
    cout << endl << setw(10) << "Штраф";
    for (int j = 0; j < 6; j++) cout << setw(10) << (cp[j] == -1 ? "X" : to_string(cp[j])) << endl << endl;
}

int main() {
    setlocale(LC_ALL, "Russian");

    vector<int> supply = { 168 + N, 113 + N, 150 + N, 159 + N, 100 + N, 143 };
    vector<int> demand = { 143 + N, 107 + N, 131 + N, 193 + N, 95 + N, 163 + N };

    vector<vector<int>> cost = {
        {12 + N, 2 + N,  6 + N,  3 + N,  11 + N, 1 + N},
        {10 + N, 0 + N,  8 + N,  5 + N,  7 + N,  13 + N},
        {1 + N,  5 + N,  11 + N, 8 + N,  2 + N,  11 + N},
        {4 + N,  10 + N, 10 + N, 3 + N,  13 + N, 2 + N},
        {3 + N,  11 + N, 9 + N,  0 + N,  10 + N, 4 + N},
        {0,      0,      0,      0,      0,      0}
    };

    int n = 6, m = 6;
    vector<vector<int>> alloc(n, vector<int>(m, 0));
    vector<bool> row_done(n, false), col_done(m, false);

    cout << "=== МЕТОД ФОГЕЛЯ С ТАБЛИЦАМИ (N = " << N << ") ===" << endl;

    int iter_count = 1;
    while (true) {
        vector<int> rp(n, -1), cp(m, -1);
        int max_penalty = -1;
        Target best = { -1, -1, INF, -1 };

        for (int i = 0; i < n; i++) {
            if (row_done[i]) continue;
            vector<int> v;
            for (int j = 0; j < m; j++) if (!col_done[j]) v.push_back(cost[i][j]);
            if (v.empty()) continue;
            sort(v.begin(), v.end());
            int p = (v.size() > 1) ? v[1] - v[0] : v[0];
            rp[i] = p;
        }

        for (int j = 0; j < m; j++) {
            if (col_done[j]) continue;
            vector<int> v;
            for (int i = 0; i < n; i++) if (!row_done[i]) v.push_back(cost[i][j]);
            if (v.empty()) continue;
            sort(v.begin(), v.end());
            int p = (v.size() > 1) ? v[1] - v[0] : v[0];
            cp[j] = p;
        }

        for (int i = 0; i < n; i++) {
            if (rp[i] == -1) continue;
            if (rp[i] > max_penalty) {
                max_penalty = rp[i];
                int min_c = INF, target_c = -1;
                for (int j = 0; j < m; j++)
                    if (!col_done[j] && cost[i][j] < min_c) { min_c = cost[i][j]; target_c = j; }
                best = { i, target_c, min_c, rp[i] };
            }
            else if (rp[i] == max_penalty) {
                int min_c = INF, target_c = -1;
                for (int j = 0; j < m; j++)
                    if (!col_done[j] && cost[i][j] < min_c) { min_c = cost[i][j]; target_c = j; }
                if (min_c < best.val) best = { i, target_c, min_c, rp[i] };
            }
        }

        for (int j = 0; j < m; j++) {
            if (cp[j] == -1) continue;
            if (cp[j] > max_penalty) {
                max_penalty = cp[j];
                int min_c = INF, target_r = -1;
                for (int i = 0; i < n; i++)
                    if (!row_done[i] && cost[i][j] < min_c) { min_c = cost[i][j]; target_r = i; }
                best = { target_r, j, min_c, cp[j] };
            }
            else if (cp[j] == max_penalty) {
                int min_c = INF, target_r = -1;
                for (int i = 0; i < n; i++)
                    if (!row_done[i] && cost[i][j] < min_c) { min_c = cost[i][j]; target_r = i; }
                if (min_c < best.val) best = { target_r, j, min_c, cp[j] };
            }
        }

        if (best.r == -1) break;

        printTable(iter_count++, cost, alloc, supply, demand, rp, cp, row_done, col_done);

        int r = best.r, c = best.c;
        int ship = min(supply[r], demand[c]);
        alloc[r][c] = ship;
        supply[r] -= ship;
        demand[c] -= ship;

        cout << ">>> Выбор: Клетка [" << r + 1 << "," << c + 1 << "] стоимость " << cost[r][c]
            << ". Распределено: " << ship << endl;

        if (supply[r] == 0) row_done[r] = true;
        else if (demand[c] == 0) col_done[c] = true;
    }

    long long Z = 0;
    cout << "\n" << string(30, '=') << " ИТОГОВЫЙ ПЛАН " << string(30, '=') << endl;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < m; j++) {
            if (alloc[i][j] > 0) {
                cout << "X_" << i + 1 << "," << j + 1 << " = " << setw(4) << alloc[i][j];
                if (i == 5) cout << " (Фиктивный)";
                cout << " | Затраты: " << alloc[i][j] << " * " << cost[i][j] << " = " << alloc[i][j] * cost[i][j] << endl;
                Z += (long long)alloc[i][j] * cost[i][j];
            }
        }
    }
    cout << string(75, '=') << endl;
    cout << "ОБЩИЕ ЗАТРАТЫ Z = " << Z << endl;

    return 0;
}