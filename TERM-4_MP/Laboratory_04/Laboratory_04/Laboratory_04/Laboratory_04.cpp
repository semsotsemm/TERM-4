#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <chrono> 
#include <iomanip> 

#define fsl 300
#define ssl 200

using namespace std;

void GenerateRandomCharacterStrings(string& s1, string& s2);
int LevenshteinDP(const string& s1, const string& s2);
int LevenshteinRecursive(const string& s1, const string& s2, int m, int n);

int main()
{
    setlocale(LC_ALL, "Russian");

    string s1, s2;
    vector<int> k_divisors = { 25, 20, 15, 10, 5, 2, 1 };

    GenerateRandomCharacterStrings(s1, s2);

    cout << "S1: " << s1.length() << " | S2: " << s2.length() << endl << endl;
    cout << fixed << setprecision(3);

    for (int k : k_divisors) {
        int len1 = s1.length() / k;
        int len2 = s2.length() / k;

        string prefix1 = s1.substr(0, len1);
        string prefix2 = s2.substr(0, len2);

        cout << ">>> k = 1/" << k << " (Размер: " << len1 << "x" << len2 << ")" << endl;

        auto startDP = chrono::high_resolution_clock::now();
        int distDP = LevenshteinDP(prefix1, prefix2);
        auto endDP = chrono::high_resolution_clock::now();

        chrono::duration<double> elapsedDP = endDP - startDP;
        double timeDP = elapsedDP.count();
        if (timeDP < 0.001) timeDP = 0.001; 

        cout << "Динамическое программирование:      " << distDP << " | " << timeDP << " с" << endl;

        auto startRec = chrono::high_resolution_clock::now();
        int distRec = LevenshteinRecursive(prefix1, prefix2, len1, len2);
        auto endRec = chrono::high_resolution_clock::now();

        chrono::duration<double> elapsedRec = endRec - startRec;
        double timeRec = elapsedRec.count();
        if (timeRec < 0.001) timeRec = 0.001;

        cout << "Рекурсия:                          " << distRec << " | " << timeRec << " с" << endl;
        cout << "--------------------------------------------" << endl;
    }

    return 0;
}

int LevenshteinDP(const string& s1, const string& s2)
{
    
    int m = s1.length();
    int n = s2.length();
    vector<vector<int>> dp(m + 1, vector<int>(n + 1, 0));
    for (int i = 0; i <= m; i++) dp[i][0] = i;
    for (int j = 0; j <= n; j++) dp[0][j] = j;

    for (int i = 1; i <= m; i++) {
        for (int j = 1; j <= n; j++) {
            int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
            dp[i][j] = min({ dp[i - 1][j] + 1, dp[i][j - 1] + 1, dp[i - 1][j - 1] + cost });
        }
    }
    return dp[m][n];
}

int LevenshteinRecursive(const string& s1, const string& s2, int m, int n)
{
    if (m == 0) return n;
    if (n == 0) return m;

    return min({
     LevenshteinRecursive(s1, s2, m - 1, n) + 1,                       
     LevenshteinRecursive(s1, s2, m, n - 1) + 1,                        
     LevenshteinRecursive(s1, s2, m - 1, n - 1) + (s1[m - 1] == s2[n - 1] ? 0 : 1)});
}

void GenerateRandomCharacterStrings(string& s1, string& s2)
{
    string chars = "qwertyuiopasdfghjklzxcvbnm";
    srand(time(0));
    for (int i = 0; i < fsl; i++) s1 += chars[rand() % chars.length()];
    for (int i = 0; i < ssl; i++) s2 += chars[rand() % chars.length()];
}