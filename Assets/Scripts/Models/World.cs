using Oasez.Extensions.Generics.Singleton;

public class World : GenericSingleton<World, World> {

    public Progression Progression = new Progression();

}