using IaActividad1.Steering.Dynamic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceExpert : Expert
{
    private List<GameObject> team; //necesitaremos la lista de agentes para comprobar su proximidad a los obstáculos
    private float avoidanceRadius; //el radio dentro del cual los agentes considerarán que un obstáculo es una amenaza y activarán la evitación
    private float offsetIntensity; //la intensidad del desplazamiento lateral que se aplicará a los agentes

    private Blackboard actualBlackboard; //lo usaremos para guardar la referencia a la blackboard, así no tenemos que pasarla como parámetro a cada acción del Run

    public ObstacleAvoidanceExpert(List<GameObject> team, float avoidanceRadius, float offsetIntensity)
    {
        //el constructor que se creará desde FormationControllerScene3, recibe la lista de agentes, el radio de evitación y la intensidad del desplazamiento lateral
        this.team = team;
        this.avoidanceRadius = avoidanceRadius;
        this.offsetIntensity = offsetIntensity;
    }

    public override float GetInsistence(Blackboard blackboard)
    {
        List<GameObject> obstacles = blackboard.GetDataByKey<List<GameObject>>("ObstacleList");//Comprobamos lablackboard y vemos si hay obstaculos
        if (obstacles == null || obstacles.Count == 0) return 0f;//Si no hay obstáculos, la insistencia es 0, el árbitro de la blackboard no le dará prioridad a este experto
        //Si hay obstáculos, comprobamos cada agente del equipo para ver si alguno está dentro del radio de evitación de algún obstáculo
        //si hay obstáculos pero no están cerca de los agentes, la insistencia también es 0
        foreach (GameObject agent in team) 
        {
            if (agent == null) continue;
            foreach (GameObject obs in obstacles)
            {
                if (obs == null) continue;
                if (Vector3.Distance(agent.transform.position, obs.transform.position) < avoidanceRadius)//si hay un agente cerca de un obstáculo se pone insistenacia alta para que se ejecute el Run del experto
                {
                    return 0.9f;
                }
            }
        }
        return 0f;
    }

    public override Action[] Run(Blackboard blackboard)
    {
        this.actualBlackboard = blackboard;//aquí le pasamos la referencia a la blackbard
        return new Action[] { ExecuteEvasion };//Aquí le pasamos el método que ejecutará la blackboard cuando sea elegido por el árbitro
    }
    private void ExecuteEvasion()
    {
        List<GameObject> obstacles = actualBlackboard.GetDataByKey<List<GameObject>>("ObstacleList");//obtenemos la lista de obstáculos de la blackboard

        if (obstacles == null) return;//volvemos a comprobar los obstáculos por si acaso

        foreach (GameObject agent in team)//cada agente de la formación comprueba la proximidad
        {
            if (agent == null) continue;
            GameObject dangerousObstacle = null; //nos aseguramos de que no hay obstáculos peligrosos antes de comprobar las distancias
            float closestDistance = avoidanceRadius;//le indicamos el radio de evitación como distancia máxima para considerar un obstáculo como peligroso

            foreach (GameObject obstacle in obstacles)
            {
                if (obstacle == null) continue;
                float distance = Vector3.Distance(agent.transform.position, obstacle.transform.position);
                if (distance < closestDistance)//comprobamos la distancia entre el agente y cada obstáculo
                {
                    closestDistance = distance;
                    dangerousObstacle = obstacle;
                }
            }

            if (dangerousObstacle != null)//si hay un obstáculo peligroso, aplicamos la lógica de evasión
            {
                SeekVirtual seek = agent.GetComponent<SeekVirtual>();//cambiamos su objetivo seek a una evasión
                if (seek != null)
                {
                    Vector3 avoidanceDirection = (agent.transform.position - dangerousObstacle.transform.position).normalized;//calculamos la dirección de evasión como el vector que va del obstáculo al agente
                    Vector3 lateralClearance = Vector3.Cross(avoidanceDirection, Vector3.up).normalized;//calculamos un vector perpendicular a la dirección de evasión para crear un desplazamiento lateral
                    seek.objetiveTarget += lateralClearance * offsetIntensity;//le pasamos un objetivo que será el resultado de sumar al objetivo actual un desplazamiento lateral en la dirección perpendicular a la evasión, multiplicado por la intensidad del desplazamiento
                }
            }
        }
    }
}