public class ItemHealth : HealthSystem
{
    public BoxType boxType;
    public override void Start()
    {
        base.Start();
        onDeathEvent.AddListener(delegate { OnDead(); });
    }


    private void OnDisable()
    {
        Setup();
    }

    void OnDead()
    {
        if (GameManager.Singleton)
        {
            switch (boxType)
            {
                case BoxType.WeaponBox:
                    GameManager.Singleton.spawnerManager.currentBoxCount--;
                    if (GameManager.Singleton.spawnerManager.currentBoxCount < 0)
                    {
                        GameManager.Singleton.spawnerManager.currentBoxCount = 0;
                    }
                    break;
                case BoxType.BuffBox:
                    GameManager.Singleton.spawnerManager.currentBuffBoxCount--;
                    if (GameManager.Singleton.spawnerManager.currentBuffBoxCount < 0)
                    {
                        GameManager.Singleton.spawnerManager.currentBuffBoxCount = 0;
                    }
                    break;
            }

        }
    }

    public enum BoxType { WeaponBox, BuffBox }
}
