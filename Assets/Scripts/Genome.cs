using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Genome
{
    //Number of genes in each half of the genome.
    public const int numGenes = 8;

    //geneAlleles contains the number of possible alleles for each gene.
    /*
     * Gene 0: A(gouti)
     * Gene 1: B(lack)
     * Gene 2: C(hin)
     * Gene 3: D(ilute)
     * Gene 4: E(xtension)
     * Gene 5: En (Broken gene)
     * Gene 6: Dwarf gene (probably remove because scaling looks bad)
     * Gene 7: V(ienna) 
     * Gene 8 will *Eventually* be lop vs nonlop ears
     */
    public static readonly int[] geneAlleles = new int[numGenes] { 3, 2, 5, 2, 4, 2, 2, 2};

    //Genome consists of a 2D array of genes. 
    //One set [0,] is inherited from the father. 
    //The other [1,] is inherited from the mother.
    public readonly int[,] genes;

    //Phenotype consists of the dominant allele of each gene.
    public int[] phenotype;

    //Seed unique to this genome that determines positioning of spots, harlequin pattern, etc.
    public int personalSeed;
    
    //Constructor with two parents.
    // Receives half genome from each parent as arguments and combines them into genes array.
    public Genome(int[] father, int[] mother)
    {
        genes = new int[2,numGenes];

        for (int i = 0; i < numGenes; i++)
        {
            genes[0,i] = father[i];
        }

        for (int i = 0; i < numGenes; i++)
        {
            genes[1,i] = mother[i];
        }

        getPhenotype();
    }

    //Constructor with single parent. 
    // Creates a genetic clone of the parent by copying the parent's entire genome.
    public Genome(int[,] original)
    {
        genes = new int[2,numGenes];

        for(int i = 0; i <= 1; i++)
        {
            for(int j = 0; j < numGenes; j++)
            {
                genes[i,j] = original[i,j];
            }
        }

        getPhenotype();
    }

    //Constructor with no parents. 
    // Generates a new, random genome.
    public Genome()
    {
        genes = new int[2,numGenes];

        for(int i = 0; i <= 1; i++)
        {
            for (int j = 0; j < numGenes; j++)
            {
                genes[i,j] = UnityEngine.Random.Range(0,geneAlleles[j]); //Loop through each gene and pick a random allele from the possible alleles.
            }
        }

        getPhenotype();
    }

    public Genome(bool initialSpawn)
    {
        if (!initialSpawn) return;
        genes = new int[2, numGenes];
        for (int i = 0; i <= 1; i++)
        {
            for (int j = 0; j < numGenes; j++)
            {
                genes[i, j] = UnityEngine.Random.Range(0, geneAlleles[j]); //Loop through each gene and pick a random allele from the possible alleles.
                if (j == 5 && genes[i, j] == 0) genes[i, j] += UnityEngine.Random.Range(0, 2) == 0 ? 1 : 0;
            }
        }

        getPhenotype(); 
    }

    //Determines the dominant allele of each gene and saves it to the phenotype.
    private void getPhenotype()
    {
        personalSeed = UnityEngine.Random.Range(0, 1000000000);
        phenotype = new int[numGenes];
        for (int i = 0; i < numGenes; i++)
        {
            //Handles double-dominants, otherwise uses normal dominance
            if ((i == 6 || i == 5) && genes[0, i] == genes[1, i] && genes[0, i] == 0) phenotype[i] = 2;
            else phenotype[i] = Math.Min(genes[0,i], genes[1,i]);
        }
    }

    //Function to get genome array.
    public int[,] sampleGenome()
    {
        return genes;
    }

}
