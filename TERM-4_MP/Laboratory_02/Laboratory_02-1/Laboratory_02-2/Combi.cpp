#include "pch.h"
#include "Combi.h"
#include <algorithm>

namespace combi
{
    Combination::Combination(short n, short m)
    {
        this->n = n;
        this->m = m;
        this->sequence = new short[m + 2];
        this->reset();
    }
    void Combination::reset()
    {
        for (int i = 0; i < this->m; i++)
        {
            this->sequence[i] = i;
        }
        this->sequence[m] = this->n;
        this->sequence[m + 1] = 0;
        // [0 1 3 | 5 0]
    };
    short Combination::getfirst()
    {
        return (this->n >= this->m) ? this->m : -1;
    };
    void Combination::getNextCombination()
    {
        short* ptr = sequence + m - 1; 

        while (ptr >= sequence)
        {
            if (*(ptr) + 1 < *(ptr + 1))
            {
                (*ptr)++;

                for (short* next = ptr + 1; next < sequence + m; ++next)
                {
                    *next = *(next - 1) + 1;
                }
                return; 
            }
            ptr--; 
        }

    };

    unsigned int Combination::count_combinations()
    {
        if (this->m > this->n)
        {
            return 0;
        }

        if (this->m == this->n || this->m == 0)
        {
            return 1;
        }

        unsigned int count = 1;

        for (int i = 1; i <= (this->n - this->m); i++)
        {
            count *= (this->m + i); 
            count /= i;             
        }
        return count;
    };
}