#include "pch.h"
#include "Combi.h"
#define RIGHT 1
#define LEFT 0

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
        this->sequence = (Element*)operator new[](size * sizeof(Element));

        for (short i = 0; i < this->sequence_size; i++)
        {
            new (&this->sequence[i]) Element(i, LEFT);
        }
    }
    int Transposition::findMaxMobile()
    {
        short maxElement = -1;
        short indexMax = -1;
        for (short i = 0; i < this->sequence_size; i++)
        {
            if (maxElement < this->sequence[i].element)
            {
                if (this->sequence[i].arrow == RIGHT)
                {
                    if (i + 1 < this->sequence_size && this->sequence[i].element > this->sequence[i + 1].element)
                    {
                        maxElement = this->sequence[i].element;
                        indexMax = i;
                    }
                }
                else
                {
                    if (i - 1 >= 0 && this->sequence[i].element > this->sequence[i - 1].element)
                    {
                        maxElement = this->sequence[i].element;
                        indexMax = i;
                    }
                }
            }
        }
        return indexMax;
    }
    int Transposition::getNextTransp()
    {
        int index = -1;
        index = findMaxMobile();
        if (index == -1)
        {
            return -1;
        }
        for (int i = 0; i < this->sequence_size; i++)
        {
            if (this->sequence[index].element < this->sequence[i].element)
            {
                this->sequence[i].arrow = !this->sequence[i].arrow;
            }
        }
        if (this->sequence[index].arrow == LEFT)
        {
            std::swap(this->sequence[index], this->sequence[index - 1]);
        }
        else
        {
            std::swap(this->sequence[index], this->sequence[index + 1]);
        }
        return index;

    }
}