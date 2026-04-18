#pragma once
#include <iostream>

namespace combi
{
    struct Transposition
    {
        struct Element
        {
            short element;
            bool arrow; 
            Element(short el, bool arr);
            Element() = default; 
        };

        short sequence_size;
        Element* sequence;

        Transposition(short size);
        ~Transposition();

        void reset();
        int getNextTransp();
        int findMaxMobile();
    };

    struct Combination
    {
        short n, m;
        short* sequence;

        Combination(short n, short m);
        ~Combination(); 

        void reset();
        int getNextCombination(); 
        unsigned __int64 count_combinations() const;
    };

    struct Accomodation
    {
        short n, m;
        Combination* cgen;  
        Transposition* pgen; 
        short* sset;        

        Accomodation(short n, short m);
        ~Accomodation();

        void reset();
        short getfirst();
        short getnext();
        unsigned __int64 count() const;
    };
}