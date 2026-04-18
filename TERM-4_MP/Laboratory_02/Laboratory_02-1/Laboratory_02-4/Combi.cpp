#include "pch.h"
#include "Combi.h"

#define LEFT false
#define RIGHT true

namespace combi
{
    Transposition::Element::Element(short el, bool arr)
    {
        this->element = el;
        this->arrow = arr;
    }

    Transposition::Transposition(short size)
    {
        this->sequence_size = size;
        this->sequence = new Element[this->sequence_size];
        this->reset();
    }

    Transposition::~Transposition()
    {
        delete[] this->sequence;
    }

    void Transposition::reset()
    {
        for (short i = 0; i < this->sequence_size; i++)
        {
            this->sequence[i] = Element(i, LEFT);
        }
    }

    int Transposition::findMaxMobile()
    {
        short maxElement = -1;
        short indexMax = -1;

        for (short i = 0; i < this->sequence_size; i++)
        {
            bool isMobile = false;
            if (this->sequence[i].arrow == LEFT) 
            {
                if (i > 0 && this->sequence[i].element > this->sequence[i - 1].element)
                    isMobile = true;
            }
            else 
            {
                if (i < this->sequence_size - 1 && this->sequence[i].element > this->sequence[i + 1].element)
                    isMobile = true;
            }

            if (isMobile && this->sequence[i].element > maxElement)
            {
                maxElement = this->sequence[i].element;
                indexMax = i;
            }
        }
        return indexMax;
    }

    int Transposition::getNextTransp()
    {
        int index = findMaxMobile();
        if (index == -1)
        {
            return -1;
        }

        short mobileVal = this->sequence[index].element;
        bool mobileArrow = this->sequence[index].arrow;

        if (mobileArrow == LEFT)
        {
            std::swap(this->sequence[index], this->sequence[index - 1]);
        }
        else
        {
            std::swap(this->sequence[index], this->sequence[index + 1]);
        }

        for (int i = 0; i < this->sequence_size; i++)
        {
            if (this->sequence[i].element > mobileVal)
            {
                this->sequence[i].arrow = !this->sequence[i].arrow;
            }
        }

        return index; 
    }

    Combination::Combination(short n, short m)
    {
        this->n = n;
        this->m = m;
        this->sequence = new short[m];
        this->reset();
    }

    Combination::~Combination()
    {
        delete[] this->sequence;
    }

    void Combination::reset()
    {
        for (short i = 0; i < this->m; i++)
        {
            this->sequence[i] = i;
        }
    }

    unsigned __int64 fact(unsigned __int64 x) 
    { 
        return (x == 0) ? 1 : (x * fact(x - 1)); 
    }

    unsigned __int64 Combination::count_combinations() const
    {
        if (m < 0 || m > n)
        {
            return 0;
        }
        return fact(n) / (fact(m) * fact(n - m));
    }

    int Combination::getNextCombination()
    {
        int i = m - 1;
        while (i >= 0 && sequence[i] >= n - m + i)
        {
            i--;
        }

        if (i < 0)
        {
            return -1; 
        }

        sequence[i]++; 

        for (int j = i + 1; j < m; j++)
        {
            sequence[j] = sequence[j - 1] + 1;
        }

        return 1; 
    }

    Accomodation::Accomodation(short n, short m)
    {
        this->n = n;
        this->m = m;
        this->cgen = new Combination(n, m);
        this->pgen = new Transposition(m);
        this->sset = new short[m];
        this->reset();
    }

    Accomodation::~Accomodation()
    {
        delete this->cgen;
        delete this->pgen;
        delete[] this->sset;
    }

    void Accomodation::reset()
    {
        this->cgen->reset();
        this->pgen->reset();
        this->getfirst();
    }

    short Accomodation::getfirst()
    {
        if (this->n < this->m) return -1;

        for (int i = 0; i < this->m; i++)
        {
            this->sset[i] = this->cgen->sequence[this->pgen->sequence[i].element];
        }
        return this->m;
    }

    short Accomodation::getnext()
    {
        short rc = -1;

        if (this->pgen->getNextTransp() != -1)
        {
            rc = this->getfirst();
        }
        else if (this->cgen->getNextCombination() != -1)
        {
            this->pgen->reset();
            rc = this->getfirst();
        }

        return rc;
    }

    unsigned __int64 Accomodation::count() const
    {
        return (this->n >= this->m) ? fact(this->n) / fact(this->n - this->m) : 0;
    }
}