#include <iostream>
#include <chrono>
#include <iomanip>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;
using namespace std::chrono;

#define BASE_STR_X "ABCDEFG"
#define BASE_STR_Y "BMDRFI"
#define REPEAT_COUNT 1

int lcs(int lenx, const char x[], int leny, const char y[]) {
    if (lenx == 0 || leny == 0) return 0;
    if (x[lenx - 1] == y[leny - 1])
        return 1 + lcs(lenx - 1, x, leny - 1, y);
    else
        return std::max(lcs(lenx, x, leny - 1, y), lcs(lenx - 1, x, leny, y));
}

string get_lcs_string(const std::vector<std::vector<int>>& f, const char x[], const char y[], int i, int j) {
    string res = "";
    while (i > 0 && j > 0) {
        if (x[i - 1] == y[j - 1]) {
            res += x[i - 1];
            i--; j--;
        }
        else if (f[i - 1][j] > f[i][j - 1]) {
            i--;
        }
        else {
            j--;
        }
    }
    reverse(res.begin(), res.end()); 
    return res;
}

pair<int, string> lcs_dynamic_with_str(int lenx, const char x[], int leny, const char y[]) {
    std::vector<std::vector<int>> f(lenx + 1, std::vector<int>(leny + 1, 0));

    for (int i = 1; i <= lenx; i++) {
        for (int j = 1; j <= leny; j++) {
            if (x[i - 1] == y[j - 1])
                f[i][j] = f[i - 1][j - 1] + 1;
            else
                f[i][j] = std::max(f[i - 1][j], f[i][j - 1]);
        }
    }

    string lcs_str = get_lcs_string(f, x, y, lenx, leny);
    return { f[lenx][leny], lcs_str };
}

int main() {
    setlocale(LC_ALL, "rus");

    string X_long = "";
    string Y_long = "";
    for (int i = 0; i < REPEAT_COUNT; i++) {
        X_long += BASE_STR_X;
        Y_long += BASE_STR_Y;
    }

    const char* X = X_long.c_str();
    const char* Y = Y_long.c_str();

    int max_len = min((int)X_long.length(), (int)Y_long.length());

    cout << "Базовая строка X: " << BASE_STR_X << endl;
    cout << "Базовая строка Y: " << BASE_STR_Y << endl;
    cout << "--------------------------------------------------------------------------------" << endl;
    cout << "k\t| Длина\t| Рекурсия (мс)\t| ДП (мс)\t| Сама последовательность" << endl;
    cout << "--------------------------------------------------------------------------------" << endl;

    for (int k = 1; k <= max_len; k++) {
        auto start_dyn = high_resolution_clock::now();
        auto res_dyn_pair = lcs_dynamic_with_str(k, X, k, Y);
        auto end_dyn = high_resolution_clock::now();
        double time_dyn = duration<double, std::milli>(end_dyn - start_dyn).count();

        auto start_rec = high_resolution_clock::now();
        int res_rec = lcs(k, X, k, Y);
        auto end_rec = high_resolution_clock::now();
        double time_rec = duration<double, std::milli>(end_rec - start_rec).count();

        cout << k << "\t| " << res_dyn_pair.first << "\t| "
            << fixed << setprecision(5) << time_rec << "\t| "
            << time_dyn << "\t| " << res_dyn_pair.second << endl;
    }

    return 0;
}