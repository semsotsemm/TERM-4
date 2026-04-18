namespace combi
{
	struct Subset
	{
		short size_set;
		int quantity_subset;
		unsigned short mask;
		int* included_elements;

		Subset(char initial_set[], short size)
		{
			size_set = size;
			quantity_subset = 1 << size_set;
			mask = 0;
			included_elements = new int[size_set];
			clearSubset();
		}

		~Subset() 
		{
			delete[] included_elements;
		}
		void clearSubset();
		void getNextSubset();
	};
}