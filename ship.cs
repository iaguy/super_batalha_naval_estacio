class Ship
{
    private int size;
    private int hits;

    public Ship(int size)
    {
        this.size = size;
        this.hits = 0;
    }

    public void Hit() => hits++;

    public bool IsSunk() => hits >= size;
}
