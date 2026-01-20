using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.ParticleSystem;

public abstract class BeastWarrior : MonoBehaviour
{
    public enum WeaponMode
    {
        None,
        Bend,
        Straight,
        Throw,
        Diagonal
    }

    public enum WeaponArm
    {
        None,
        Right,
        Left,
        Both
    }

    protected Character character;

    protected Camera characterCamera;

    protected Animator animator;

    protected int weapon;

    protected float[] cameraPosition;

    protected bool lightShoot;

    protected bool heavyShoot;

    protected int barrel;

    protected bool right;

    protected bool left;

    protected void Awake()
    {
        cameraPosition = new float[4];
        cameraPosition[0] = 0f;
        cameraPosition[1] = 0f;
        cameraPosition[2] = -2.5f;
        cameraPosition[3] = -2.5f;
        weapon = 1;
        lightShoot = false;
        heavyShoot = false;
    }

    protected void Start()
    {
        character = transform.parent.gameObject.GetComponent<Character>();
        characterCamera = transform.parent.GetComponent<Character>().characterCamera;
        animator = transform.parent.GetComponentInChildren<Animator>();
        animator.enabled = false;
    }

    protected void FixedUpdate()
    {
        float x = characterCamera.transform.localPosition.x;
        if (characterCamera.transform.localPosition.x > cameraPosition[weapon - 1])
        {
            x = Mathf.Clamp(x - 0.1f, cameraPosition[weapon - 1], x);
        }
        else if (characterCamera.transform.localPosition.x < cameraPosition[weapon - 1])
        {
            x = Mathf.Clamp(x + 0.1f, x, cameraPosition[weapon - 1]);
        }
        characterCamera.transform.localPosition = new Vector3(x, characterCamera.transform.localPosition.y, characterCamera.transform.localPosition.z);
    }

    protected void RaycastBullet(GameObject bullet, Vector3 direction, LayerMask layerMask, GameObject barrel, bool audio = true)
    {
        Quaternion rotation = Quaternion.Euler(-characterCamera.transform.eulerAngles.x, transform.parent.eulerAngles.y, 0f);
        Vector3 target = rotation * direction;
        if (Physics.Raycast(characterCamera.transform.position, target, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject b = Instantiate(bullet, barrel.transform.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f));
            b.GetComponent<AudioSource>().enabled = audio;
            GameObject h = b.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            Debug.DrawLine(barrel.transform.position, hit.point, Color.darkBlue, 3600);
            Debug.DrawRay(characterCamera.transform.position, target * hit.distance, Color.blue, 3600);
        }
    }

    protected void RaycastLaser(LineRenderer laser, Vector3 direction, LayerMask layerMask, GameObject barrel, Color color)
    {
        Quaternion rotation = Quaternion.Euler(-characterCamera.transform.eulerAngles.x, transform.parent.eulerAngles.y, 0f);
        Vector3 target = rotation * direction;
        if (Physics.Raycast(characterCamera.transform.position, target, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            LineRenderer l = Instantiate(laser);
            l.transform.position = barrel.transform.position;
            l.SetPosition(0, Vector3.zero);
            l.SetPosition(1, hit.point - barrel.transform.position);
            l.startColor = color;
            l.endColor = color;
            l.material.SetColor("_Color", color);
            GameObject f = l.transform.GetChild(0).gameObject;
            f.GetComponent<Light>().color = color;
            MainModule m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(color);
            GameObject h = l.transform.GetChild(1).gameObject;
            h.transform.position = hit.point;
            h.GetComponent<Light>().color = color;
            m = h.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(color);
            Debug.DrawLine(barrel.transform.position, hit.point, Color.darkRed, 3600);
            Debug.DrawRay(characterCamera.transform.position, target * hit.distance, Color.red, 3600);
        }
    }

    protected void ParticleProjectile(GameObject flash, GameObject projectile, Vector3 direction, LayerMask layerMask,
        GameObject barrel, Color flashColor, Color projectileColor, Gradient gradient)
    {
        Quaternion rotation = Quaternion.Euler(-characterCamera.transform.eulerAngles.x, transform.parent.eulerAngles.y, 0f);
        Vector3 target = rotation * direction;
        if (Physics.Raycast(characterCamera.transform.position, target, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject f = Instantiate(flash, barrel.transform.position, Quaternion.LookRotation(hit.point - barrel.transform.position));
            f.GetComponent<Light>().color = flashColor;
            MainModule m = f.GetComponent<ParticleSystem>().main;
            m.startColor = new MinMaxGradient(flashColor); 
            GameObject p = Instantiate(projectile, barrel.transform.position, Quaternion.LookRotation(-hit.point + barrel.transform.position));
            p.GetComponent<Light>().color = projectileColor;
            ColorOverLifetimeModule c = p.GetComponent<ParticleSystem>().colorOverLifetime;
            c.color = new MinMaxGradient(gradient);
            Debug.DrawLine(barrel.transform.position, hit.point, Color.darkGreen, 3600);
            Debug.DrawRay(characterCamera.transform.position, target * hit.distance, Color.green, 3600);
        }
    }

    protected void MeshProjectile(GameObject flash, GameObject projectile, Vector3 direction, Vector3 orientation,
        LayerMask layerMask, GameObject barrel, Color flashColor, Material material)
    {
        Quaternion rotation = Quaternion.Euler(-characterCamera.transform.eulerAngles.x, transform.parent.eulerAngles.y, 0f);
        Vector3 target = rotation * direction;
        if (Physics.Raycast(characterCamera.transform.position, target, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject e = Instantiate(flash, barrel.transform.position, Quaternion.LookRotation(hit.point - barrel.transform.position));
            GameObject m = Instantiate(projectile, barrel.transform.position, Quaternion.LookRotation(-hit.point + barrel.transform.position));
            m.transform.Rotate(orientation);
            m.GetComponentInChildren<MeshRenderer>().material = material;
            if (flashColor != Color.clear)
            {
                e.GetComponent<Light>().color = flashColor;
                MainModule p = e.GetComponent<ParticleSystem>().main;
                p.startColor = new MinMaxGradient(flashColor);
                m.GetComponent<Light>().color = flashColor;
            }
            Debug.DrawLine(barrel.transform.position, hit.point, Color.darkCyan, 3600);
            Debug.DrawRay(characterCamera.transform.position, target * hit.distance, Color.cyan, 3600);
        }
    }

    protected void ThrownProjectile(GameObject thrown, GameObject projectile, Vector3 direction,
        Vector3 orientation, LayerMask layerMask, GameObject hold, bool spin)
    {
        Quaternion rotation = Quaternion.Euler(-characterCamera.transform.eulerAngles.x, transform.parent.eulerAngles.y, 0f);
        Vector3 target = rotation * direction;
        if (Physics.Raycast(characterCamera.transform.position, target, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            GameObject t = Instantiate(thrown, hold.transform.position, Quaternion.LookRotation(-hit.point + hold.transform.position));
            GameObject p = Instantiate(projectile, t.transform);
            t.transform.localScale = transform.parent.localScale;
            t.transform.Rotate(orientation);
            Thrown s = t.GetComponent<Thrown>();
            s.forward = (hit.point - hold.transform.position).normalized;
            s.spin = spin;
            BoxCollider tc = t.GetComponent<BoxCollider>();
            BoxCollider pc = projectile.GetComponent<BoxCollider>();
            tc.center = new Vector3(pc.center.x, pc.center.y, -pc.center.z);
            tc.size = pc.size;
            Debug.DrawLine(hold.transform.position, hit.point, Color.darkMagenta, 3600);
            Debug.DrawRay(characterCamera.transform.position, target * hit.distance, Color.magenta, 3600);
        }
    }

    protected void InitThrower(GameObject thrower, Color[] flameColors)
    {
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[3];
        colors[0].color = flameColors[0];
        colors[0].time = 0f;
        colors[1].color = flameColors[1];
        colors[1].time = 0.5f;
        colors[2].color = flameColors[2];
        colors[2].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[3];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        colors[1].time = 0.5f;
        alphas[2].alpha = 0f;
        alphas[2].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleSystem p = thrower.GetComponent<ParticleSystem>();
        ColorOverLifetimeModule c = p.GetComponent<ParticleSystem>().colorOverLifetime;
        c.color = new MinMaxGradient(g);
        var lights = thrower.GetComponentsInChildren<Light>();
        lights[0].color = flameColors[0];
        lights[1].color = flameColors[2];
    }

    protected void ShootMachineGun(WeaponArm arm, GameObject bullet, GameObject[] barrels, float bulletInaccuracy, int shots = 1)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Both:
                animator.SetBool("Right", right);
                animator.SetBool("Left", left);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction;
        for (int i = 0; i < shots; i++)
        {
            direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), -1f);
            RaycastBullet(bullet, direction, layerMask, barrels[barrel + i * barrels.Length / 2]);
        }
        barrel = barrel == barrels.Length / shots - 1 ? 0 : barrel + 1;
        if (arm == WeaponArm.Both)
        {
            right = !right;
            left = !left;
        }
    }

    protected void ShootMachineGun(WeaponArm arm, GameObject bullet, GameObject barrel, float bulletInaccuracy)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), -1f);
        RaycastBullet(bullet, direction, layerMask, barrel);
    }

    protected bool ShootBall(WeaponArm arm, GameObject flash, GameObject ball, GameObject[] barrels, Color flashColor, Color ballColor)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Both:
                animator.SetBool("Right", right);
                animator.SetBool("Left", left);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(0f, 0f, -1f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = flashColor;
        colors[0].time = 0f;
        colors[1].color = ballColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(flash, ball, direction, layerMask, barrels[barrel], flashColor, ballColor, g);
        barrel = barrel == barrels.Length - 1 ? 0 : barrel + 1;
        if (arm == WeaponArm.Both)
        {
            right = !right;
            left = !left;
        }
        return false;
    }

    protected bool ShootBall(WeaponArm arm, GameObject flash, GameObject ball, GameObject barrel, Color flashColor, Color ballColor)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(0f, 0f, -1f);
        Gradient g = new();
        GradientColorKey[] colors = new GradientColorKey[2];
        colors[0].color = flashColor;
        colors[0].time = 0f;
        colors[1].color = ballColor;
        colors[1].time = 1f;
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0].alpha = 1f;
        alphas[0].time = 0f;
        alphas[1].alpha = 1f;
        alphas[1].time = 1f;
        g.SetKeys(colors, alphas);
        ParticleProjectile(flash, ball, direction, layerMask, barrel, flashColor, ballColor, g);
        return false;
    }

    protected bool ShootLaser(WeaponArm arm, LineRenderer laser, GameObject[] barrels, Color laserColor, float laserInaccuracy, int shots = 1)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Both:
                animator.SetBool("Right", right);
                animator.SetBool("Left", left);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction;
        for (int i = 0; i < shots; i++)
        {
            direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), -1f);
            RaycastLaser(laser, direction, layerMask, barrels[barrel + i * barrels.Length / 2], laserColor);
        }
        barrel = barrel == barrels.Length / shots - 1 ? 0 : barrel + 1;
        if (arm == WeaponArm.Both)
        {
            right = !right;
            left = !left;
        }
        return false;
    }

    protected bool ShootLaser(WeaponArm arm, LineRenderer laser, GameObject barrel, Color laserColor, float laserInaccuracy)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(Random.Range(-laserInaccuracy, laserInaccuracy), Random.Range(-laserInaccuracy, laserInaccuracy), -1f);
        RaycastLaser(laser, direction, layerMask, barrel, laserColor);
        return false;
    }

    protected bool ShootBolt(WeaponArm arm, GameObject flash, GameObject bolt, GameObject[] barrels, Material boltMaterial, Color boltColor, float angle = 0f)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Both:
                animator.SetBool("Right", right);
                animator.SetBool("Left", left);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(0f, 0f, -1f);
        Vector3 orientation = new(0f, 0f, angle);
        Material m = new(boltMaterial);
        if (boltColor != Color.clear)
        {
            m.color = boltColor;
        }
        MeshProjectile(flash, bolt, direction, orientation, layerMask, barrels[barrel], boltColor, m);
        barrel = barrel == barrels.Length - 1 ? 0 : barrel + 1;
        if (arm == WeaponArm.Both)
        {
            right = !right;
            left = !left;
        }
        return false;
    }

    protected bool ShootBolt(WeaponArm arm, GameObject flash, GameObject bolt, GameObject barrel, Material boltMaterial, Color boltColor, float angle = 0f)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(0f, 0f, -1f);
        Vector3 orientation = new(0f, 0f, angle);
        Material m = new(boltMaterial);
        if (boltColor != Color.clear)
        {
            m.color = boltColor;
        }
        MeshProjectile(flash, bolt, direction, orientation, layerMask, barrel, boltColor, m);
        return false;
    }

    protected bool Throw(WeaponArm arm, GameObject thrown, GameObject projectile, GameObject[] barrels, float x, float y, bool spin = false)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Both:
                animator.SetBool("Right", right);
                animator.SetBool("Left", left);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(0f, 0f, -1f);
        Vector3 orientation = new(x, y, 0f);
        ThrownProjectile(thrown, projectile, direction, orientation, layerMask, barrels[barrel], spin);
        barrel = barrel == barrels.Length - 1 ? 0 : barrel + 1;
        if (arm == WeaponArm.Both)
        {
            right = !right;
            left = !left;
        }
        return false;
    }

    protected bool Throw(WeaponArm arm, GameObject thrown, GameObject projectile, GameObject barrel, float x, float y, bool spin = false)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Vector3 direction = new(0f, 0f, -1f);
        Vector3 orientation = new(x, y, 0f);
        ThrownProjectile(thrown, projectile, direction, orientation, layerMask, barrel, spin);
        return false;
    }

    protected bool ShootShotgun(WeaponArm arm, GameObject bullet, GameObject slug, GameObject[] barrels, float bulletInaccuracy, int slugCount)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Both:
                animator.SetBool("Right", right);
                animator.SetBool("Left", left);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Instantiate(slug, barrels[barrel].transform.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f));
        Vector3 direction;
        for (int i = 0; i < slugCount; i++)
        {
            direction = new Vector3(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), -1f);
            RaycastBullet(bullet, direction, layerMask, barrels[barrel], false);
        }
        barrel = barrel == barrels.Length - 1 ? 0 : barrel + 1;
        if (arm == WeaponArm.Both)
        {
            right = !right;
            left = !left;
        }
        return false;
    }

    protected bool ShootShotgun(WeaponArm arm, GameObject bullet, GameObject slug, GameObject barrel, float bulletInaccuracy, int slugCount)
    {
        int layerMask = 1 << 3;
        layerMask = ~layerMask;
        switch (arm)
        {
            case WeaponArm.Right:
                animator.SetBool("Right", true);
                animator.SetBool("Left", false);
                animator.SetTrigger("Shoot");
                break;
            case WeaponArm.Left:
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                animator.SetTrigger("Shoot");
                break;
        }
        Instantiate(slug, barrel.transform.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f));
        Vector3 direction;
        for (int i = 0; i < slugCount; i++)
        {
            direction = new Vector3(Random.Range(-bulletInaccuracy, bulletInaccuracy), Random.Range(-bulletInaccuracy, bulletInaccuracy), -1f);
            RaycastBullet(bullet, direction, layerMask, barrel, false);
        }
        return false;
    }

    protected void Equip(GameObject weapon, GameObject attachment)
    {
        weapon.transform.parent = attachment.transform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localEulerAngles = Vector3.zero;
    }

    protected void Deploy(GameObject weapon, float xAngle, float yAngle, float zAngle)
    {
        weapon.transform.localEulerAngles = new Vector3(xAngle, yAngle, zAngle);
    }

    public virtual void OnMeleeWeak(CallbackContext context)
    {
        character.Crosshair.enabled = false;
        lightShoot = false;
        heavyShoot = false;
    }

    public virtual void OnMeleeStrong(CallbackContext context)
    {
        character.Crosshair.enabled = false;
        lightShoot = false;
        heavyShoot = false;
    }

    public virtual void OnRangedWeak(CallbackContext context)
    {
        character.Crosshair.enabled = true;
        lightShoot = false;
        heavyShoot = false;
    }

    public virtual void OnRangedStrong(CallbackContext context)
    {
        character.Crosshair.enabled = true;
        lightShoot = false;
        heavyShoot = false;
    }

    public abstract void OnAttack(CallbackContext context);
}
