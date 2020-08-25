using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Genome
{
    //Number of genes in each half of the genome.
    public const int numGenes = 1; //Will be at least 8 genes when properly implemented  

    //geneAlleles contains the number of possible alleles for each gene.
    public static readonly int[] geneAlleles = new int[1] { 2 };

    //Genome consists of a 2D array of genes. 
    //One set [0,] is inherited from the father. 
    //The other [1,] is inherited from the mother.
    int[,] genes;

    //Phenotype consists of the dominant allele of each gene.
    public int[] phenotype;
    
    //Constructor with two parents.
    // Receives half genome from each parent as arguments and combines them into genes array.
    public Genome(int[] father, int[] mother)
    {
        genes = new int[2, numGenes];

        for (int i = 0; i < numGenes; i++)
        {
            genes[0,    i] = father[i];
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
        genes = new int[2, numGenes];

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
        genes = new int[2, numGenes];

        for(int i = 0; i <= 1; i++)
        {
            for (int j = 0; j < numGenes; j++)
            {
                genes[i,j] = UnityEngine.Random.Range(0,geneAlleles[0]); //Loop through each gene and pick a random allele from the possible alleles.
                Debug.Log(genes[i, j] + " i" + i + " j" + j);
            }
        }

        getPhenotype();
    }

    //Determines the dominant allele of each gene and saves it to the phenotype.
    private void getPhenotype()
    {
        phenotype = new int[numGenes];
        for (int i = 0; i < numGenes; i++)
        {
            Debug.Log(" 1:" + genes[0, i] + " 2:" + genes[1, i] + " min:" + Math.Min(genes[0, i], genes[1, i]));
            phenotype[i] = Math.Min(genes[0,i], genes[1,i]);
        }
    }

    //Function to get genome array.
    public int[,] sampleGenome()
    {
        return genes;
    }

}
