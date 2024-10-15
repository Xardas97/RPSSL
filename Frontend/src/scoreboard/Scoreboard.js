import ScoreItem from "./ScoreItem";
import "./styles.css";

export default function Scoreboard({shapes, gameRecords, onDeleteRecords}) {
  function getShapeName(shapeId) {
    return shapes.find(s => s.id == shapeId).name
  }

  let scoreItems = <></>
  if (gameRecords) {
    scoreItems = gameRecords.map(record =>
        <ScoreItem key={record.id} record={record} getShapeName={getShapeName}/>
    )
  }

  return (
    <div className="scoreboard">
      <div className="scoreboard-header">
        <div className="scoreboard-header-title">Scoreboard</div>
        <button className="scoreboard-header-button" onClick={onDeleteRecords}>
          <img className="scoreboard-header-button-image" src={require("./images/delete.png")}></img>
        </button>
      </div>
      <div>
        {scoreItems}
      </div>
    </div>
  )
}
