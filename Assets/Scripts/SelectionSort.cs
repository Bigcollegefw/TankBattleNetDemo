public class Sort
{
    void SelectionSort(int[] array)
    {
        int n = array.Length;
        for(int i = 0; i < n; i++)
        {
            int min = i; 
            for(int j = i + 1; j < n; j++)
            {
                
                if(array[j] < array[i])
                {
                    min = j;
                }
            }
            swap(array[i],array[min]);
        }
    }

    void Main()
    {
        int[] arr = {5,2,9,3,3,3};
        SelectionSort(arr);
        Console.WriteLine();
    }



}