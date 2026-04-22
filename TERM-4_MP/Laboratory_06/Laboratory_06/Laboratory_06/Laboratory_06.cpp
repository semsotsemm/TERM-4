#include "stdafx.h"

#define SIZE 7

using namespace std;

struct Edge
{
	int u, v, weight;
};

struct DSU {
	int parent[SIZE];
	
	DSU() 
	{
		for (int i = 0; i < SIZE; i++) parent[i] = i;
	}
	
	int find(int i) 
	{
		if (parent[i] == i)
		{
			return i;
		}
		return parent[i] = find(parent[i]);
	}
	
	void unite(int i, int j) 
	{
		int root_i = find(i);
		int root_j = find(j);
		if (root_i != root_j) parent[root_i] = root_j;
	}
};

struct AMatrix
{
	bool data[SIZE][SIZE];
};

struct AList
{
	vector<int> adj[SIZE];
};
	
void dfs(bool matrix[SIZE][SIZE], bool visited[SIZE], int u, vector<int>& visited_edge);
void topologicalSortUtil(bool matrix[SIZE][SIZE], bool visited[SIZE], stack<int>& s, int u);
void topologicalSort(bool matrix[SIZE][SIZE]);
void bfs(bool matrix[SIZE][SIZE], int startNode);
AList convertToAList(const AMatrix& m);
AMatrix convertToAMatrix(const AList& l);
int minKey(int key[], bool mstSet[]);
void primMST(short graph[SIZE][SIZE]);
bool compareEdges(Edge a, Edge b);
void kruskalMST(short graph[SIZE][SIZE]);

int main()
{
	setlocale(LC_ALL, "Russian");
	vector<int> visited_edge;
	bool graph_matrix[SIZE][SIZE] =
	{
	//   0  1  2  3  4  5  6
		{0, 1, 0, 0, 0, 0, 0},
		{0, 0, 0, 1, 1, 0, 0},
		{0, 0, 0, 0, 0, 0, 0},
		{0, 0, 1, 0, 0, 0, 1},
		{0, 0, 0, 0, 0, 0, 0},
		{0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 0},
	};
	/*
	bool visited[SIZE] = {false};
	
	cout << "Обход в глубину начат с вершины 0.\n";
	visited_edge.push_back(0);
	dfs(graph_matrix, visited, 0, visited_edge);
	cout << "Порядок обхода вершин: ";
	
	for (int i = 0; i < visited_edge.size(); i++)
	{
		cout << visited_edge[i];
		if (i < visited_edge.size() - 1)
		{
			cout << ", ";
		}
		else
		{
			cout << '.';
		}
	}
	*/

	//cout << "Обход в ширину начат с вершины 0. ";
	//bfs(graph_matrix, 0);
	//cout << endl;

	cout << "Топологическая сортировка: ";
	topologicalSort(graph_matrix);
	cout << "null" << endl;


	//primMST(second_graph_matrix);
	//kruskalMST(second_graph_matrix);
	return 0;
}

void dfs(bool matrix[SIZE][SIZE], bool visited[SIZE], int u, vector<int>& visited_edge) {
	visited[u] = true;

	cout << "Посетили узел: " << u << endl;

	for (int v = 0; v < SIZE; v++)
	{
		if (matrix[u][v] && !visited[v])
		{
			visited_edge.push_back(v);
			dfs(matrix, visited, v, visited_edge);

		}
	}
}

void bfs(bool matrix[SIZE][SIZE], int startNode) {
	bool visited[SIZE] = { false };
	queue<int> q;
	vector<int> visited_edge;

	visited[startNode] = true;
	q.push(startNode);

	while (!q.empty()) 
	{
		int u = q.front();
		q.pop();
		cout  << "Посещена вершина: " << u << "\n";

		for (int v = 0; v < SIZE; v++) 
		{
			if (matrix[u][v] && !visited[v]) 
			{
				visited[v] = true;
				q.push(v);
				visited_edge.push_back(v);
			}
		}
	}
	
	cout << "Порядок обхода вершин: ";
	for (int i = 0; i < visited_edge.size(); i++)
	{
		cout << visited_edge[i];
		if (i < visited_edge.size() - 1)
		{
			cout << ", ";
		}
		else 
		{
			cout << '.';
		}
	}
}

void topologicalSortUtil(bool matrix[SIZE][SIZE], bool visited[SIZE], stack<int>& s, int u) {
	visited[u] = true;
	for (int v = 0; v < SIZE; v++) 
	{
		if (matrix[u][v] && !visited[v]) 
		{
			topologicalSortUtil(matrix, visited, s, v);
		}
	}
	s.push(u);
}

void topologicalSort(bool matrix[SIZE][SIZE]) {
	stack<int> s;
	bool visited[SIZE] = { false };

	for (int i = 0; i < SIZE; i++) 
	{
		if (!visited[i]) 
		{
			topologicalSortUtil(matrix, visited, s, i);
		}
	}

	while (!s.empty()) 
	{
		cout << s.top() << "->";
		s.pop();
	}
}

AList convertToAList(const AMatrix& m)
{
	AList l;
	for (int i = 0; i < SIZE; i++)
	{
		for (int j = 0; j < SIZE; j++)
		{
			if (m.data[i][j])
			{
				l.adj[i].push_back(j);
			}
		}
	}
	return l;
}

AMatrix convertToAMatrix(const AList& l)
{
	AMatrix m;
	for (int i = 0; i < SIZE; i++)
	{
		for (int j = 0; j < SIZE; j++)
		{
			m.data[i][j] = false;
		}

		for (int neighbor : l.adj[i])
		{
			m.data[i][neighbor] = true;
		}
	}
	return m;
}

int minKey(int key[], bool mstSet[]) {
	int min = INT_MAX, min_index;

	for (int v = 0; v < SIZE; v++)
	{
		if (mstSet[v] == false && key[v] < min)
		{
			min = key[v], min_index = v;
		}
	}
	return min_index;
}

void primMST(short graph[SIZE][SIZE]) {
	int parent[SIZE];
	int key[SIZE];
	bool mstSet[SIZE];

	for (int i = 0; i < SIZE; i++)
	{
		key[i] = INT_MAX;
		mstSet[i] = false;
	}

	key[0] = 0;
	parent[0] = -1;

	for (int count = 0; count < SIZE - 1; count++)
	{
		int u = minKey(key, mstSet);

		mstSet[u] = true;

		for (int v = 0; v < SIZE; v++)
		{
			if (graph[u][v] && mstSet[v] == false && graph[u][v] < key[v])
			{
				parent[v] = u;
				key[v] = graph[u][v];
			}
		}
	}

	cout << "Edge \tWeight" << endl;
	int totalWeight = 0;
	for (int i = 1; i < SIZE; i++)
	{
		cout << parent[i] << " - " << i << " \t" << graph[parent[i]][i] << endl;
		totalWeight += graph[parent[i]][i];
	}
	cout << "Total MST Weight: " << totalWeight << endl;
}

bool compareEdges(Edge a, Edge b) 
{
	return a.weight < b.weight;
}

void kruskalMST(short graph[SIZE][SIZE]) 
{
	vector<Edge> edges;

	for (int i = 0; i < SIZE; i++) 
	{
		for (int j = 0; j < SIZE; j++) 
		{
			if (graph[i][j] != 0) 
			{
				edges.push_back({ i, j, graph[i][j] });
			}
		}
	}

	sort(edges.begin(), edges.end(), compareEdges);

	DSU dsu;
	vector<Edge> mst;
	int mst_weight = 0;

	cout << "Алгоритм Краскала (выбор ребер):" << endl;

	for (Edge& e : edges) 
	{
		if (dsu.find(e.u) != dsu.find(e.v)) 
		{
			dsu.unite(e.u, e.v);
			mst.push_back(e);
			mst_weight += e.weight;
			cout << "Ребро " << e.u << " - " << e.v << " (вес: " << e.weight << ") добавлено." << endl;
		}
	}

	cout << "\nИтоговое дерево (MST) вес: " << mst_weight << endl;
}