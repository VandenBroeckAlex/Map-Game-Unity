using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Parliment
{
    /*
    Parliament {
    seats: 100,
    parties: [Liberal, Monarchist, Socialist, Technocrat],
    seat_distribution: {
      Liberal: 35,
      Monarchist: 40,
      Socialist: 15,
      Technocrat: 10
    },
    active_laws: [...],
    pending_reforms: [...]
    }



     def hold_election():
      pop_votes = defaultdict(int)
      for pop in all_pops:
          ideology = pop.ideology
          pop_votes[ideology] += pop.size * pop.literacy # educated pops vote more effectively

      total_votes = sum(pop_votes.values())
      parliament.seat_distribution = {
          ideology: int((votes / total_votes) * parliament.seats)
          for ideology, votes in pop_votes.items()
      
    law = {
    "name": "Abolish Serfdom",
    "support": {
        "Liberal": +10,
        "Monarchist": -15,
        "Socialist": +5,
        "Technocrat": +2
    },
    "passed": False
}

def vote_on_law(parliament, law):
    total_support = sum([
        parliament.seat_distribution[party] * law["support"].get(party, 0)
        for party in parliament.parties
    ])
    if total_support > passing_threshold:
        law["passed"] = True

    if law["name"] == "Abolish Serfdom":
    for pop in all_pops:
        if pop.class == "Peasant":
            pop.wealth += 5
            pop.political_leaning["Liberal"] += 0.1

    1    Pop Update Phase

            Update needs, wealth, literacy

            Update political leanings

            Some Pops change dominant ideology

     2   Faction Update Phase

            Recalculate strength of parties based on pop support

            Determine emerging ideologies or radical groups

     3   Parliament Phase (if it's election time or a law is proposed)

            Run election or simulate votes

            Check for passed laws, apply effects

            If gridlock, trigger unrest or crises



    */
    public class PoliticalFaction
    {
        public string Name;
        public Ideology Ideology;
        public string Description;
        public List<string> CoreDemands;
        public List<string> OpposedFactions;
    }
    public class InterestGroup
    {
        public string Name;
        public string ClassAlignment; // e.g. "Nobility", "Workers", "Scientists"
        public List<Ideology> IdeologicalLeanings;
        public string Description;
        public int PowerLevel; // Influence in % or scale 0-100
    }
    PoliticalFaction onarchists = new PoliticalFaction
        {
            Name = "Royal Loyalists",
            Ideology = Ideology.Monarchist,
            Description = "Support the divine right of kings and aristocratic rule.",
            CoreDemands = new List<string> { "Preserve Monarchy", "Suppress Elections", "Support Church" },
            OpposedFactions = new List<string> { "Liberals", "Socialists", "Anarchists" }
        };

    PoliticalFaction liberals = new PoliticalFaction
    {
        Name = "Constitutional Reformists",
        Ideology = Ideology.Liberal,
        Description = "Advocate for individual freedoms, constitutions, and property rights.",
        CoreDemands = new List<string> { "Voting Rights", "Free Market", "Limit Monarchy" },
        OpposedFactions = new List<string> { "Monarchists", "Socialists" }
    };

    PoliticalFaction socialists = new PoliticalFaction
    {
        Name = "Workers' Vanguard",
        Ideology = Ideology.Socialist,
        Description = "Push for wealth redistribution, labor rights, and social equality.",
        CoreDemands = new List<string> { "Abolish Serfdom", "Unionize Industry", "Expand Welfare" },
        OpposedFactions = new List<string> { "Monarchists", "Capitalists", "Theocrats" }
    };

    PoliticalFaction theocrats = new PoliticalFaction
    {
        Name = "Divine Order",
        Ideology = Ideology.Theocrat,
        Description = "Believe in governance by religious doctrine and divine law.",
        CoreDemands = new List<string> { "State Religion", "Censorship", "Ban Secular Education" },
        OpposedFactions = new List<string> { "Liberals", "Socialists", "Technocrats" }
    };

    PoliticalFaction nationalists = new PoliticalFaction
    {
        Name = "Blood & Soil Union",
        Ideology = Ideology.Nationalist,
        Description = "Seek to unify or purify the nation based on ethnicity or history.",
        CoreDemands = new List<string> { "Military Expansion", "Suppress Minorities", "Annex Territories" },
        OpposedFactions = new List<string> { "Anarchists", "Liberals", "Socialists" }
    };

}

public enum Ideology
{
    Monarchist,
    Liberal,
    Socialist,
    Theocrat,
    Nationalist,
    Technocrat,
    Anarchist,
    Capitalist,
    Populist,
    Reactionary,
    Communism,
    SteampunkUtopian,
    SteampunkDominionist
}


/*
private void Update_political_leaning(pop)
{
// Factor: Wealth and Class
    if pop.class == "Worker" and pop.wealth<poverty_threshold:
    pop.political_leaning["Socialist"] += 0.01

        pop.political_leaning["Monarchist"] -= 0.005

# Factor: Literacy and Events
if pop.literacy > 0.5:

        pop.political_leaning["Liberal"] += 0.005

# Factor: Propaganda
if province.propaganda["Monarchist"] > 0:

        pop.political_leaning["Monarchist"] += 0.01

# Normalize to cap and set dominant ideology
    normalize_political_leanings(pop)
}

*/