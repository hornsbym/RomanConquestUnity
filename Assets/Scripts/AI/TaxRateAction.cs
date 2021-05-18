public class TaxRateAction : Action
{
    private City city;
    private float rate;

    public TaxRateAction (City city, float rate) 
    {
        this.city = city;
        this.rate = rate;
    }

    override public void Execute()
    {
        city.taxRate = rate;
    }

    override public bool IsPossible() 
    {
        if (city.taxRate == rate){
            return false;
        } else {
            return true;
        }
    }

    override public void Evaluate() 
    {
        value = 1;
    }
}