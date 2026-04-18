#include "pch.h"
#include "Combi.h"

namespace combi
{
    void Subset::clearSubset() 
    {
        for (int i = 0; i < this->size_set; i++) 
        {
            this->included_elements[i] = 0;
        }
    }
    void Subset::getNextSubset() 
    {
        unsigned short buf = this->mask;
        for (int j = 0; j < size_set; j++)
        {
            if (buf & 0x01)
            {
                *(included_elements + j) = j + 1;
            }
            buf >>= 1;
        }
    }
}