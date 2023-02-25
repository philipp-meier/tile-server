import http from "k6/http";

export let options = {
    insecureSkipTLSVerify: true,
    scenarios: {
        stress: {
            executor: "ramping-arrival-rate",
            preAllocatedVUs: 500,
            timeUnit: "1s",
            stages: [
                { duration: "30s", target: 10 },
                { duration: "1m", target: 20 },
                { duration: "1m", target: 30 },
                { duration: "30s", target: 40 },
                { duration: "30s", target: 0 } // Recovery stage.
            ]
        }
    }
};

export default function() {
    http.get("https://localhost/5/15/19.png");
}