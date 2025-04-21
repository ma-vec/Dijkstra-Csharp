using System;
using System.Collections.Generic;

public class Program
{
    const int numberVertex = 5;
    public interface IGraph
    {
        void SetWeight(int u, int v, double w);
        List<int> GetVertices();
        List<int> GetNeighbors(int vertex);
        double GetWeight(int u, int v);
    }

    public class SimpleGraph : IGraph
    {
        private const double NF = double.PositiveInfinity;
        private double[,] Table;

        public SimpleGraph(int numVertices)
        {
            Table = new double[numVertices, numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = 0; j < numVertices; j++)
                {
                    Table[i, j] = NF;
                }
            }
        }

        public List<int> GetVertices()
        {
            var result = new List<int>();
            for (int i = 0; i < Table.GetLength(0); i++)
            {
                result.Add(i + 1);
            }
            return result;
        }

        public List<int> GetNeighbors(int vertex)
        {
            var result = new List<int>();
            for (int i = 0; i < Table.GetLength(0); i++)
            {
                if (Table[vertex - 1, i] < NF && vertex - 1 != i)
                {
                    result.Add(i + 1);
                }
            }
            return result;
        }

        public void SetWeight(int u, int v, double w)
        {
            Table[u - 1, v - 1] = w;
        }

        public double GetWeight(int u, int v)
        {
            return Table[u - 1, v - 1];
        }
    }

    // Funzione per eseguire l'algoritmo di Dijkstra
    public static void Dijkstra(SimpleGraph graph, int source)
    {
        int numVertices = graph.GetVertices().Count;

        // Inizializzazione delle distanze e dei predecessori
        double[] dist = new double[numVertices];
        int[] prev = new int[numVertices];
        for (int i = 0; i < numVertices; i++) //ripete ciò che è nel costruttore
        {
            dist[i] = double.PositiveInfinity;
            prev[i] = -1;
        }

        dist[source - 1] = 0;

        // Coda di priorità per i nodi da visitare
        PriorityQueue<int, double> queue = new PriorityQueue<int, double>();
        queue.Enqueue(source, 0);

        // Algoritmo di Dijkstra (tipo wikipedia)
        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            foreach (int neighbor in graph.GetNeighbors(u))
            {
                double alt = dist[u - 1] + graph.GetWeight(u, neighbor);

                if (alt < dist[neighbor - 1]) //se il percorso alternativo, passando per altri nodi è migliore di quello trovato da
                {
                    dist[neighbor - 1] = alt;
                    prev[neighbor - 1] = u;
                    queue.Enqueue(neighbor, alt);
                }
            }
        }

        // Stampa i risultati
        Console.WriteLine("\nDistanze minime dal nodo " + source + ":");
        for (int i = 0; i < numVertices; i++)
        {
            Console.WriteLine($"Nodo {i + 1}: distanza = {dist[i]}");
        }

        // Stampa i percorsi minimi
        Console.WriteLine("\nPercorsi minimi:");
        for (int i = 0; i < numVertices; i++)
        {
            if (i + 1 == source) continue; //esclude il primo (sorgente)

            //lo stack è necessario perchè non viene calcolato il percorso minimo da Dijkstra ma solo le distanze minimie salvate in
            //dist ed eventualmente aggiornate con alt. Lo stack permette di risalire il percorso al contrario visto che "inverte" il percorso

            Console.Write($"Percorso verso {i + 1}: ");
            var path = new Stack<int>(); 
            int current = i + 1;

            while (prev[current - 1] != -1)
            {
                path.Push(current);
                current = prev[current - 1];
            }
            path.Push(source);

            Console.WriteLine(string.Join(" tramite: ", path));
        }
    }



    public static void Main(string[] args)
    {
        SimpleGraph g = new SimpleGraph(numberVertex);

        g.SetWeight(1, 2, 1);
        g.SetWeight(1, 3, 2);
        g.SetWeight(2, 4, 1);
        g.SetWeight(3, 4, 2);
        g.SetWeight(3, 5, 2);

        
        Dijkstra(g, 1);

    }
}
